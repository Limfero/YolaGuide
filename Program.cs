using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Enums;
using YolaGuide.Controllers;

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
                                           text: Answer.WrongInputFormat[(int)Settings.Language],
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
                                   text: Answer.WrongInputFormat[(int)Settings.Language],
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
                    await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Выберете язык:\nSelect a language:",
                       cancellationToken: cancellationToken,
                       replyMarkup: Keyboard.LanguageChange);
                    break;

                default:
                    switch (user.State)
                    {
                        case State.Start:
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: Answer.WrongCommand[(int)Settings.Language],
                                cancellationToken: cancellationToken);
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
                    if (callbackQuery.Data == "English")
                        Settings.Language = Language.English;
                    else
                        Settings.Language = Language.Russian;

                    if (Settings.Admins.Contains(chatId))
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectAdmin[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectAdministrator());

                        break;
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart());
                    break;

                case "Главное меню":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart());
                    break;

                case "Админ панель":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WhatToAdd[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd());
                    break;

                case "Место":
                    user.State = State.AddPlace;
                    user.StateAdd = StateAdd.GettingPlaceName;
                    await UserController.UpdateUser(user);

                    PlaceController.AddNewPairInDictionary(chatId);

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceName[(int)Settings.Language],
                        cancellationToken: cancellationToken);
                    break;

                case "Категорию":
                    user.State = State.AddCategory;
                    user.StateAdd = StateAdd.GettingCategoryName;
                    await UserController.UpdateUser(user);

                    CategoryController.AddNewPairInDictionary(chatId);

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringCategoryName[(int)Settings.Language],
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
                    }
                    break;
            }
        }
    }
}