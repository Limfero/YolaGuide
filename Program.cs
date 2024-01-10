using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.Service.Interfaces;
using YolaGuide.Service.Implementation;
using Microsoft.Extensions.Logging;

namespace YolaGuide
{
    public class Program
    {      
        public static async Task Main()
        {
            Setup();

            var _botClient = new TelegramBotClient(Settings.Token);

            using CancellationTokenSource cancellationTokenSource = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationTokenSource.Token
            );

            var me = await _botClient.GetMeAsync();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Старт бота под имененем @{me.Username}");
            Console.ResetColor();
            Console.ReadLine();

            cancellationTokenSource.Cancel();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var message = update.Message;
                        var chatId = update.Message.Chat.Id;
                        var user = Settings.UserController.GetUserById(chatId);

                        switch (message.Type)
                        {
                            case MessageType.Text:
                                await ReplyToTextMessageAsync(botClient, message, cancellationToken, user);
                                break;
                            case MessageType.Photo:
                                switch (user.State)
                                {
                                    case State.Start:
                                        await botClient.SendTextMessageAsync(
                                           chatId: chatId,
                                           text: Answer.WrongInputType[(int)user.Language],
                                           cancellationToken: cancellationToken);
                                        break;

                                    case State.AddPlace:
                                        await Settings.PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                                        break;

                                    case State.AddFact:
                                        await Settings.FactController.AddFactAsync(botClient, message, cancellationToken, user);
                                        break;
                                }
                                break;

                            default:
                                await botClient.SendTextMessageAsync(
                                   chatId: chatId,
                                   text: Answer.WrongInputType[(int)user.Language],
                                   cancellationToken: cancellationToken);
                                break;
                        }
                        break;

                    case UpdateType.CallbackQuery:
                        user = Settings.UserController.GetUserById(update.CallbackQuery.Message.Chat.Id);

                        await ReplyToCallbackQueryAsync(botClient, update, cancellationToken, user);
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибочка: {ex}");
                Console.ResetColor();
            }
        }

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static async Task ReplyToTextMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;
            var messageText = message.Text;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now.TimeOfDay} -> Получено сообщение '{messageText}' в чате {chatId} от {message.Chat.FirstName}.");
            Console.ResetColor();

            switch (messageText.ToLower())
            {
                case "/start":
                    if (user == null)
                    {
                        if (!Settings.LastBotMsg.ContainsKey(chatId))
                            Settings.LastBotMsg.Add(chatId, null);

                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Выберете язык:\nSelect a language:",
                           parseMode: ParseMode.Html,
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.LanguageChange);
                        break;
                    }

                    await ResettingUserStates(user);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ReplyMenu(chatId, user.Language));
                    break;

                case "настройки ⚙️":
                case "settings ⚙️":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.Settings[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetSettings(user.Language));
                    break;

                case "админ панель 😎":
                case "admin panel 😎":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectAdmin[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectToDeleteOrAdd(user.Language, ""));
                    break;

                case "готовые маршруты 🛣️":
                case "prepared routes 🛣️":
                    user.State = State.GetRotes;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.RouteController.GetAllRoutes(botClient, message, cancellationToken, user);
                    break;

                case "о городе 🏙️":
                case "about the city 🏙️":
                    user.State = State.AboutCity;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.AboutCity(botClient, cancellationToken, user, message);
                    break;

                case "найти места 🔍":
                case "find places 🔍":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectingMenuButton[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.InlineMenu(user.Language));
                    break;

                case "запланировать поездку 👜":
                case "plan a trip 👜":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.Blank[(int)user.Language],
                       parseMode: ParseMode.Html,
                       cancellationToken: cancellationToken,
                       replyMarkup: Keyboard.Back(user.Language));
                    break;

                case "афиша 📃":
                case "playbill 📃":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.Playbill[(int)user.Language],
                       parseMode: ParseMode.Html,
                       cancellationToken: cancellationToken,
                       replyMarkup: Keyboard.Back(user.Language));
                    break;

                case "для маломобильного гостя 🦿":
                case "for a guest with low mobility 🦿":
                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.Blank[(int)user.Language],
                       parseMode: ParseMode.Html,
                       cancellationToken: cancellationToken,
                       replyMarkup: Keyboard.Back(user.Language));
                    break;

                default:
                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                               chatId: chatId,
                               text: Answer.WrongCommand[(int)user.Language],
                               parseMode: ParseMode.Html,
                               cancellationToken: cancellationToken);
                        break;
                    }

                    List<State> statesNoWrite = new() {
                        State.AddPlaceToPlan,
                        State.ClarificationOfPreferences,
                        State.Start
                    };

                    if (statesNoWrite.Contains(user.State)) 
                    {
                        Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                            chatId: user.Id,
                            text: Answer.WrongCommand[(int)user.Language],
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.Back(user.Language));

                        break; 
                    }

                    await ChangeState(botClient, message, cancellationToken, user);
                    break;
            }
        }

        private static async Task ReplyToCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var callbackQuery = update.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
            var message = new Message() { Text = callbackQuery.Data, Chat = callbackQuery.Message.Chat, MediaGroupId = callbackQuery.Id };

            var callback = callbackQuery.Data.Split("\n");

            var callbackData = callback[0];
            var additionalInformation = "";

            if (callback.Length == 2)
                additionalInformation = callback[1];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now.TimeOfDay} -> {message.Chat.FirstName} нажал на кнопку '{callbackData}' в чате {chatId}.");
            Console.ResetColor();

            switch (callbackData)
            {
                case "English":
                case "Русский":
                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)(callbackData == "Русский" ? Language.Russian : Language.English)],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ReplyMenu(chatId, callbackData == "Русский" ? Language.Russian : Language.English));

                        await Settings.CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user, callbackData == "Русский" ? Language.Russian : Language.English);
                        break;
                    }

                    await ChangeLanguage(user, callbackQuery.Data);

                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfulLanguageChange[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ReplyMenu(chatId, user.Language));

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.Loading[(int)user.Language],
                       parseMode: ParseMode.Html,
                       cancellationToken: cancellationToken);

                    goto case "Открыть меню";

                case "Уточнение предпочтений":
                    user.Categories = new();
                    user.State = State.ClarificationOfPreferences;
                    user.Substate = Substate.Start;

                    await Settings.UserController.UpdateUser(user);

                    await Settings.CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user, user.Language);
                    break;

                case "В план на сегодня":
                    await InPlan(botClient, message, cancellationToken, user);
                    break;

                case "Дай факт!":
                    await botClient.DeleteMessageAsync(
                       chatId: user.Id,
                       messageId: Settings.LastBotMsg[user.Id].MessageId);

                    var fact = Settings.FactController.GetRandomFact();

                    if (fact == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: Answer.NothingToOffer[(int)user.Language],
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.Back(user.Language));

                        break;
                    }

                    using (var fileStream = new FileStream(Settings.DestinationImagePath + fact.Image, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Settings.LastBotMsg[user.Id] = await botClient.SendPhotoAsync(
                            chatId: user.Id,
                            photo: InputFile.FromStream(fileStream),
                            caption: Answer.GetFactInformation(fact, user.Language),
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.Fact(user.Language));
                    }
                    break;

                case "Посоветуй место!":
                    var place = Settings.PlaceController.GetRandomPlace(user);

                    if (place == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.NothingToOffer[(int)user.Language],
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.Back(user.Language));

                        break;
                    }

                    user.Substate = Substate.End;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;

                case "План на сегодня":
                    await InPlan(botClient, message, cancellationToken, user);
                    break;

                case "Поиск":
                    user.State = State.Search;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.Search(botClient, message, cancellationToken, user);
                    break;

                case "Уточнение языка":
                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: "Выберете язык:\nSelect a language:",
                           parseMode: ParseMode.Html,
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.LanguageChange);
                    break;

                case "Открыть меню":
                    await ResettingUserStates(user);

                    if (Settings.DeleteMessage != null)
                    {
                        await botClient.DeleteMessageAsync(
                            chatId: user.Id,
                            messageId: Settings.DeleteMessage.MessageId);

                        Settings.DeleteMessage = null;
                    }

                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectingMenuButton[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.InlineMenu(user.Language));
                    break;

                case "Добавить":
                case "Удалить":
                    if (additionalInformation == "plan")
                        if (callbackData == "Добавить")
                            goto case "Добавить в план";
                        else
                            goto case "Удалить из плана";

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.ObjectSelection[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd(user.Language, callbackData));
                    break;

                case "В план":
                    place = Settings.PlaceController.GetPlaceById(int.Parse(additionalInformation));
                    place.Categories = new();
                    place.Routes = new();

                    user.Places.Add(place);

                    user.Substate = Substate.End;
                    await Settings.UserController.UpdateUser(user);

                    await botClient.AnswerCallbackQueryAsync(message.MediaGroupId, Answer.Added[(int)user.Language]);
                    await Settings.PlaceController.GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;

                case "Контактная информация":
                case "Маршруты":
                case "Места рядом":
                case "Похожие места":
                case "Карточка места":
                    place = Settings.PlaceController.GetPlaceById(int.Parse(additionalInformation));

                    if(callbackData == "Контактная информация") user.Substate = Substate.GettingPlaceInformation;
                    else if(callbackData == "Маршруты") user.Substate = Substate.GettingPlaceRoutes;
                    else if (callbackData == "Места рядом") user.Substate = Substate.GettingPlacesNearby;
                    else if (callbackData == "Похожие места") user.Substate = Substate.GettingSimilarPlaces;
                    else if (callbackData == "Назад") user.Substate = Substate.End;

                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;

                case "Добавить место":
                    user.State = State.AddPlace;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    Settings.PlaceController.AddNewPairInDictionary(chatId);

                    await Settings.PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить категорию":
                    user.State = State.AddCategory;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    Settings.CategoryController.AddNewPairInDictionary(chatId);

                    await Settings.CategoryController.AddCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить факт":
                    user.State = State.AddFact;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    Settings.FactController.AddNewPairInDictionary(chatId);

                    await Settings.FactController.AddFactAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить маршрут":
                    user.State = State.AddRoute;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    Settings.RouteController.AddNewPairInDictionary(chatId);

                    await Settings.RouteController.AddRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить место":
                    user.State = State.DeletePlace;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.DeletePlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить категорию":
                    user.State = State.DeleteCategory;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.CategoryController.DeleteCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить факт":
                    user.State = State.DeleteFact;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.FactController.DeleteFactAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить маршрут":
                    user.State = State.DeleteRoute;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.RouteController.DeleteRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case "Редактировать план":
                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SelectAdmin[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectToDeleteOrAdd(user.Language, "plan"));
                    break;

                case "Добавить в план":
                    user.State = State.AddPlaceToPlan;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    Settings.PlaceController.AddNewPairInDictionary(chatId);

                    await Settings.PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить из плана":
                    user.State = State.DeletePlaceInPlan;
                    user.Substate = Substate.Start;
                    await Settings.UserController.UpdateUser(user);

                    await Settings.PlaceController.DeletePlaceInPlan(botClient, message, cancellationToken, user);
                    break;

                case "Очистить план":
                    user.Places = new();
                    await Settings.UserController.UpdateUser(user);

                    await ResettingUserStates(user);
                    goto default;

                case "Я все выбрал!":
                    user.Substate = Substate.End;
                    await Settings.UserController.UpdateUser(user);
                    goto default;

                case "Назад":
                    List<Substate> cardSubstation = new()
                    {
                        Substate.GettingSimilarPlaces,
                        Substate.GettingPlacesNearby,
                        Substate.GettingPlaceInformation,
                        Substate.GettingPlaceRoutes,
                        Substate.GettingPlaceNavigation,
                        Substate.GettingPlaceRoutesNavigation,
                    };

                    if (cardSubstation.Contains(user.Substate))
                        goto case "Карточка места";

                    await BackState(user);
                    goto default;

                default:
                    if (user.Substate == Substate.GettingPlaceRoutesNavigation || user.Substate == Substate.GettingPlaceNavigation)
                        await Settings.PlaceController.GetPlaceCard(botClient, cancellationToken, Settings.PlaceController.GetPlaceById(int.Parse(callbackData)), user, message);

                    await ChangeState(botClient, message, cancellationToken, user);
                    break;
            }
        }

        private static async Task ChangeLanguage(Domain.Entity.User user, string language)
        {
            switch (language)
            {
                case "English":
                    user.Language = Language.English;
                    break;

                case "Русский":
                    user.Language = Language.Russian;
                    break;

                default:
                    throw new Exception(Answer.WrongLanguage[(int)user.Language]);
            }

            await Settings.UserController.UpdateUser(user);
        }

        private static async Task BackState(Domain.Entity.User user)
        {
            if (user.Substate == Substate.Start)
            {
                await ResettingUserStates(user);

                return;
            }

            user.Substate--;

            if (!Enum.IsDefined(user.Substate))
                user.Substate = Substate.Start;

            await Settings.UserController.UpdateUser(user);
        }

        private static async Task ChangeState(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            switch (user.State)
            {
                case State.Start:
                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                        chatId: user.Id,
                        text: Answer.SelectingMenuButton[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ReplyMenu(user.Id, user.Language));
                    break;

                case State.AddPlace:
                    await Settings.PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddCategory:
                    await Settings.CategoryController.AddCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddFact:
                    await Settings.FactController.AddFactAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddRoute:
                    await Settings.RouteController.AddRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteRoute:
                    await Settings.RouteController.DeleteRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteFact:
                    await Settings.FactController.DeleteFactAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeletePlace:
                    await Settings.PlaceController.DeletePlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteCategory:
                    await Settings.CategoryController.DeleteCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case State.ClarificationOfPreferences:
                    await Settings.CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user, user.Language);
                    break;

                case State.AddPlaceToPlan:
                    await Settings.PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);
                    break;

                case State.Search:
                    await Settings.PlaceController.Search(botClient, message, cancellationToken, user);
                    break;

                case State.GetPlan:
                    await Settings.PlaceController.GetPlan(botClient, message, cancellationToken, user);
                    break;

                case State.DeletePlaceInPlan:
                    await Settings.PlaceController.DeletePlaceInPlan(botClient, message, cancellationToken, user);
                    break;

                case State.GetRotes:
                    await Settings.RouteController.GetAllRoutes(botClient, message, cancellationToken, user);
                    break;

                case State.AboutCity:
                    await Settings.PlaceController.AboutCity(botClient, cancellationToken, user, message);
                    break;
            }
        }

        private static async Task ResettingUserStates(Domain.Entity.User user)
        {
            user.State = State.Start;
            user.Substate = Substate.Start;

            await Settings.UserController.UpdateUser(user);
        }

        private static async Task InPlan(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            if (user.Places.Count == 0)
            {
                user.State = State.AddPlaceToPlan;
                user.Substate = Substate.Start;
                await Settings.UserController.UpdateUser(user);

                Settings.PlaceController.AddNewPairInDictionary(user.Id);

                await Settings.PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);

                return;
            }

            user.State = State.GetPlan;
            user.Substate = Substate.Start;
            await Settings.UserController.UpdateUser(user);

            await Settings.PlaceController.GetPlan(botClient, message, cancellationToken, user);
        }

        private static void Setup()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IFactRepository, FactRepository>();
            builder.Services.AddScoped<IFactService, FactService>();

            builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
            builder.Services.AddScoped<IPlaceService, PlaceService>();

            builder.Services.AddScoped<IRouteRepository, RouteRepository>();
            builder.Services.AddScoped<IRouteService, RouteService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Settings.ConnectionString);
                options.LogTo(Console.WriteLine, LogLevel.Warning);
            });

            builder.Logging.ClearProviders();

            var host = builder.Build();

            var provider = host.Services.CreateScope().ServiceProvider;

            Settings.UserController = new(provider.GetService<IUserService>());
            Settings.FactController = new(provider.GetService<IFactService>());
            Settings.PlaceController = new(provider.GetService<IPlaceService>());
            Settings.CategoryController = new(provider.GetService<ICategoryService>());
            Settings.RouteController = new(provider.GetService<IRouteService>());
        }
    }
}