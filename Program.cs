using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Enums;
using YolaGuide.Controllers;
using Telegram.Bot.Types.ReplyMarkups;

namespace YolaGuide
{
    public class Program
    {
        public static async Task Main()
        {
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

            Console.WriteLine($"Старт бота под имененем @{me.Username}");
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
                        var user = UserController.GetUserById(chatId);

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
                                        await PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
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
                        user = UserController.GetUserById(update.CallbackQuery.Message.Chat.Id);

                        await ReplyToCallbackQueryAsync(botClient, update, cancellationToken, user);
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибочка: {ex}");
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

            Console.WriteLine($"Получено сообщение '{messageText}' в чате {chatId} от {message.Chat.FirstName}.");

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
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.LanguageChange);
                        break;
                    }

                    await ResettingUserStates(user);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;

                case "настройки":
                case "settings":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.Settings[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetSettings(user.Language));
                    break;

                case "админ панель":
                case "admin panel":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectAdmin[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectToDeleteOrAdd(user.Language, "admin"));
                    break;

                case "дай факт!":
                case "give me a fact!":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    var fact = FactController.GetRandomFact();

                    string? information;
                    if (fact == null)
                        information = Answer.NothingToOffer[(int)user.Language];
                    else
                        information = Answer.GetFactInformation(fact, user.Language);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: information,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.Back(user.Language));
                    break;

                case "посоветуй место!":
                case "recommend a place!":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    var place = PlaceController.GetRandomPlace(user);

                    if (place == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.NothingToOffer[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.Back(user.Language));

                        break;
                    }

                    await PlaceController.GetPlaceCard(botClient, cancellationToken, place, user);
                    break;

                case "план на сегодня":
                case "today's plan":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    if (user.Places.Count == 0)
                    {
                        user.State = State.AddPlaceToPlan;
                        user.Substate = Substate.Start;
                        await UserController.UpdateUser(user);

                        PlaceController.AddNewPairInDictionary(chatId);

                        await PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);
                        break;
                    }

                    user.State = State.GetPlan;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await PlaceController.GetPlan(botClient, message, cancellationToken, user);
                    break;

                case "готовые маршруты":
                case "prepared routes":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    user.State = State.GetRotes;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await RouteController.GetAllRoutes(botClient, message, cancellationToken, user);
                    break;

                case "поиск":
                case "search":
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    user.State = State.Search;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await PlaceController.Search(botClient, message, cancellationToken, user);
                    break;

                default:
                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                               chatId: chatId,
                               text: Answer.WrongCommand[(int)user.Language],
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

            switch (callbackQuery.Data.Split("\n")[0])
            {
                case "English":
                case "Русский":
                    if (user == null)
                    {
                        await UserController.CreateUser(new Domain.ViewModel.UserViewModel() { Id = chatId, Username = callbackQuery.From.Username });
                        user = UserController.GetUserById(chatId);

                        user.State = State.ClarificationOfPreferences;
                        user.Substate = Substate.Start;

                        await CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user);
                    }

                    await ChangeLanguage(user, callbackQuery.Data);
                    goto case "Главное меню";

                case "Уточнение предпочтений":
                    user.State = State.ClarificationOfPreferences;
                    user.Substate = Substate.Start;

                    await CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user);
                    break;

                case "Уточнение языка":
                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: "Выберете язык:\nSelect a language:",
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.LanguageChange);
                    break;

                case "Главное меню":
                    await ResettingUserStates(user);

                    await botClient.DeleteMessageAsync(
                        chatId: chatId,
                        messageId: Settings.LastBotMsg[chatId].MessageId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectingMenuButton[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;

                case "Добавить":
                case "Удалить":
                    if (callbackQuery.Data.Split("\n")[1] == "plan")
                        if (callbackQuery.Data.Split("\n")[0] == "Добавить")
                            goto case "Добавить в план";
                        else
                            goto case "Удалить из плана";

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.ObjectSelection[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd(user.Language, callbackQuery.Data));
                    break;

                case "Добавить место":
                    user.State = State.AddPlace;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    PlaceController.AddNewPairInDictionary(chatId);

                    await PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить категорию":
                    user.State = State.AddCategory;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    CategoryController.AddNewPairInDictionary(chatId);

                    await CategoryController.AddCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить факт":
                    user.State = State.AddFact;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    FactController.AddNewPairInDictionary(chatId);

                    await FactController.AddFactAsync(botClient, message, cancellationToken, user);
                    break;

                case "Добавить маршрут":
                    user.State = State.AddRoute;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    RouteController.AddNewPairInDictionary(chatId);

                    await RouteController.AddRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить место":
                    user.State = State.DeletePlace;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await PlaceController.DeletePlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить категорию":
                    user.State = State.DeleteCategory;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await CategoryController.DeleteCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить факт":
                    user.State = State.DeleteFact;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await RouteController.DeleteRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить маршрут":
                    user.State = State.DeleteRoute;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await PlaceController.DeletePlaceAsync(botClient, message, cancellationToken, user);
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
                    await UserController.UpdateUser(user);

                    PlaceController.AddNewPairInDictionary(chatId);

                    await PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);
                    break;

                case "Удалить из плана":
                    user.State = State.DeletePlaceInPlan;
                    user.Substate = Substate.Start;
                    await UserController.UpdateUser(user);

                    await PlaceController.DeletePlaceInPlan(botClient, message, cancellationToken, user);
                    break;

                case "Очистить план":
                    user.Places = new();

                    await ResettingUserStates(user);
                    goto default;

                case "Я все выбрал!":
                    user.Substate = Substate.End;
                    await UserController.UpdateUser(user);
                    goto default;

                case "Назад":
                    await BackState(user);
                    goto default;

                default:
                    if (user.Substate != Substate.End && callbackQuery.Data != "Назад")
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, Answer.Added[(int)user.Language]);

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

            await UserController.UpdateUser(user);
        }

        private static async Task CloseReplyMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, Language language)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: Answer.Loading[(int)language],
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await botClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: sentMessage.MessageId);
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

            await UserController.UpdateUser(user);
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
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(user.Id, user.Language));
                    break;

                case State.AddPlace:
                    await PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddCategory:
                    await CategoryController.AddCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddFact:
                    await FactController.AddFactAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddRoute:
                    await RouteController.AddRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteRoute:
                    await RouteController.DeleteRouteAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteFact:
                    await FactController.DeleteFactAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeletePlace:
                    await PlaceController.DeletePlaceAsync(botClient, message, cancellationToken, user);
                    break;

                case State.DeleteCategory:
                    await CategoryController.DeleteCategoryAsync(botClient, message, cancellationToken, user);
                    break;

                case State.ClarificationOfPreferences:
                    await CategoryController.ClarificationOfPreferencesAsync(botClient, message, cancellationToken, user);
                    break;

                case State.AddPlaceToPlan:
                    await PlaceController.AddPlaceInPlanAsync(botClient, message, cancellationToken, user);
                    break;

                case State.Search:
                    await PlaceController.Search(botClient, message, cancellationToken, user);
                    break;

                case State.GetPlan:
                    await PlaceController.GetPlan(botClient, message, cancellationToken, user);
                    break;

                case State.DeletePlaceInPlan:
                    await PlaceController.DeletePlaceInPlan(botClient, message, cancellationToken, user);
                    break;
            }
        }

        private static async Task ResettingUserStates(Domain.Entity.User user)
        {
            user.State = State.Start;
            user.Substate = Substate.Start;

            await UserController.UpdateUser(user);
        }
    }
}