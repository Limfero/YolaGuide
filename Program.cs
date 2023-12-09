using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Response;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Entity;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Service;

namespace YolaGuide
{
    public class Program
    {
        private static readonly string _token = "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4";
        private static readonly List<long> _admins = new() { 1059169240 };
        private static readonly UserRepository _userRepository = new(new ApplicationDbContext(new()));

        private static ITelegramBotClient? _botClient;
        private static Language _language = Language.Russian;

        private static Dictionary<long, Place> _newUserPlaces = new();

        public static async Task Main()
        {
            _botClient = new TelegramBotClient(_token);

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
                        var user = UserService.GetUsers(_userRepository).Data.FirstOrDefault(user => user.Id == chatId);

                        if (user == null)
                            await UserService.CreateUser(new Domain.ViewModel.UserViewModel() { Id = chatId, Username = update.Message.Chat.Username }, _userRepository);

                        switch (message.Type)
                        {
                            case MessageType.Text:
                                await ReplyToTextMessage(botClient, message, cancellationToken, user);
                                return;
                            default:
                                await botClient.SendTextMessageAsync(
                                   chatId: chatId,
                                   text: "Красивое... Но, к сожалению, я понимаю только текст :с",
                                   cancellationToken: cancellationToken);
                                break;
                        }
                        return;

                    case UpdateType.CallbackQuery:
                        user = UserService.GetUsers(_userRepository).Data.FirstOrDefault(user => user.Id == update.CallbackQuery.Message.Chat.Id);

                        await ReplyToCallbackQuery(botClient, update, cancellationToken, user);
                        return;
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

        private static async Task ReplyToTextMessage(ITelegramBotClient botClient, Telegram.Bot.Types.Message message, CancellationToken cancellationToken, Domain.Entity.User user)
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

                    return;
                default:
                    switch (user.State)
                    {
                        case State.Start:
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "К сожалению, я ещё тупенький и не знаю этой команды",
                                cancellationToken: cancellationToken);
                            break;
                        case State.AddPlace:
                            AddPlace(botClient, message, cancellationToken, user);
                            break;
                    }
                    return;
            }
        }

        private static async Task ReplyToCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var callbackQuery = update.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);

            switch (callbackQuery.Data)
            {
                case "English":
                case "Русский":
                    if (callbackQuery.Data == "English")
                        _language = Language.English;
                    else
                        _language = Language.Russian;

                    if (_admins.Contains(chatId))
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.SelectAdmin[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectAdministrator(_language));

                        return;
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.WelcomeMessage[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(_language));
                    return;

                case "Главное меню":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.WelcomeMessage[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(_language));
                    break;

                case "Админ панель":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.WhatToAdd[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd(_language));
                    break;

                case "Место":
                    user.State = State.AddPlace;
                    user.StateAdd = StateAdd.GettingPlaceName;
                    await _userRepository.UpdateAsync(user);
                    _newUserPlaces.Add(chatId, new());

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.EnteringPlaceName[(int)_language],
                        cancellationToken: cancellationToken);
                    break;
            }
            return;
        }

        private static async void AddPlace(ITelegramBotClient botClient, Telegram.Bot.Types.Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.GettingPlaceName:
                    _newUserPlaces[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Response.Message.EnteringPlaceDescription[(int)_language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceDescription:
                    _newUserPlaces[chatId].Description = userInput;
                    user.StateAdd = StateAdd.GettingPlaceImage;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"{_newUserPlaces[chatId].Name} {_newUserPlaces[chatId].Description}",
                        cancellationToken: cancellationToken);
                    break;
            };

            await _userRepository.UpdateAsync(user);
        }
    }
}