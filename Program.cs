using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Enums;
using YolaGuide.Controllers;
using System.Threading;
using YolaGuide.Domain.Entity;
using Telegram.Bot.Types.ReplyMarkups;
using System.Runtime.CompilerServices;

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

                        if (user == null)
                            await UserController.CreateUser(new Domain.ViewModel.UserViewModel() { Id = chatId, Username = update.Message.Chat.Username });

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
                        text: Answer.WhatToAdd[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd(user.Language));
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
                        replyMarkup: Keyboard.BackToMenu(user.Language));
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
                        replyMarkup: Keyboard.BackToMenu(user.Language));

                        break;
                    }

                    await PlaceController.GetPlaceCard(botClient, cancellationToken, place, user);
                    break;

                default:
                    await CloseReplyMenu(botClient, chatId, cancellationToken, user.Language);

                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                               chatId: chatId,
                               text: Answer.WrongCommand[(int)user.Language],
                               cancellationToken: cancellationToken);
                        break;
                    }

                    switch (user.State)
                    {
                        case State.Start:
                            Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: Answer.WrongCommand[(int)user.Language],
                                cancellationToken: cancellationToken,
                                replyMarkup: Keyboard.BackToMenu(user.Language));
                            break;

                        case State.AddPlace:
                            await PlaceController.AddPlaceAsync(botClient, message, cancellationToken, user);
                            break;

                        case State.AddCategory:
                            await CategoryController.AddCategoryAsync(botClient, message, cancellationToken, user);
                            break;
                    }
                    break;
            }
        }

        private static async Task ReplyToCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var callbackQuery = update.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);

            switch (callbackQuery.Data)
            {
                case "English":
                case "Русский":
                    await ChangeLanguage(user, callbackQuery.Data);
                    goto case "Главное меню";

                case "Уточнение пердпочтений":
                    user.State = State.ClarificationOfPreferences;
                    user.StateAdd = StateAdd.GettingPreferencesCategories;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: Answer.ClarificationOfPreferences[(int)user.Language],
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.PreferenceSelection(null, user.Language));
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
                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.WelcomeMessage[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.ClarificationPreferences(user.Language));

                        break;
                    }

                    await botClient.DeleteMessageAsync(
                        chatId: chatId,
                        messageId: Settings.LastBotMsg[chatId].MessageId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectingMenuButton[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;

                case "Место":
                    user.State = State.AddPlace;
                    user.StateAdd = StateAdd.GettingPlaceName;
                    await UserController.UpdateUser(user);

                    PlaceController.AddNewPairInDictionary(chatId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case "Категорию":
                    user.State = State.AddCategory;
                    user.StateAdd = StateAdd.GettingCategoryName;
                    await UserController.UpdateUser(user);

                    CategoryController.AddNewPairInDictionary(chatId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringCategoryName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case "Факт":
                    user.State = State.AddFact;
                    user.StateAdd = StateAdd.GettingFactName;
                    await UserController.UpdateUser(user);

                    FactController.AddNewPairInDictionary(chatId);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringFactName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case "Я все выбрал!":
                    user.StateAdd = StateAdd.End;
                    await UserController.UpdateUser(user);
                    goto default;

                default:
                    if (user.StateAdd != StateAdd.End)
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Добавлено!");

                    switch (user.State)
                    {
                        case State.AddPlace:
                            await PlaceController.AddPlaceAsync(botClient, new Message() { Text = callbackQuery.Data, Chat = callbackQuery.Message.Chat }, cancellationToken, user);
                            break;

                        case State.AddCategory:
                            await CategoryController.AddCategoryAsync(botClient, new Message() { Text = callbackQuery.Data, Chat = callbackQuery.Message.Chat }, cancellationToken, user);
                            break;

                        case State.ClarificationOfPreferences:
                            await CategoryController.ClarificationOfPreferencesAsync(botClient, new Message() { Text = callbackQuery.Data, Chat = callbackQuery.Message.Chat }, cancellationToken, user);
                            break;
                    }
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
    }
}