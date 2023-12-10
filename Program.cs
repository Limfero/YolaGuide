using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Enums;
using YolaGuide.Service;
using System.Reflection;
using YolaGuide.Domain.ViewModel;
using System.Threading;

namespace YolaGuide
{
    public class Program
    {
        private static readonly string _token = "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4";
        private static readonly List<long> _admins = new() { 1059169240 };
        private static readonly string _currentDirectory = Assembly.GetExecutingAssembly().Location;
        private static readonly string _destinationFilePath = _currentDirectory[0.._currentDirectory.IndexOf("YolaGuide")] + "YolaGuide\\Image\\";
        private static readonly Dictionary<long, PlaceViewModel> _newUserPlaces = new();

        private static Language _language = Language.Russian;

        public static async Task Main()
        {
            var _botClient = new TelegramBotClient(_token);

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
                        var user = UserService.GetUserById(chatId).Data;

                        if (user == null)
                            await UserService.CreateUser(new Domain.ViewModel.UserViewModel() { Id = chatId, Username = update.Message.Chat.Username });

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
                                           text: "Красивое... Но, к сожалению, я понимаю только текст :с",
                                           cancellationToken: cancellationToken);
                                        break;

                                    case State.AddPlace:
                                        await AddPlaceAsync(botClient, message, cancellationToken, user);
                                        break;
                                }
                                break;

                            default:
                                await botClient.SendTextMessageAsync(
                                   chatId: chatId,
                                   text: "Красивое... Но, к сожалению, я понимаю только текст :с",
                                   cancellationToken: cancellationToken);
                                break;
                        }
                        break;

                    case UpdateType.CallbackQuery:
                        user = UserService.GetUsers().Data.FirstOrDefault(user => user.Id == update.CallbackQuery.Message.Chat.Id);

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
                                text: "К сожалению, я ещё тупенький и не знаю этой команды",
                                cancellationToken: cancellationToken);
                            break;

                        case State.AddPlace:
                            await AddPlaceAsync(botClient, message, cancellationToken, user);
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
                        _language = Language.English;
                    else
                        _language = Language.Russian;

                    if (_admins.Contains(chatId))
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SelectAdmin[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectAdministrator(_language));

                        break;
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(_language));
                    break;

                case "Главное меню":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(_language));
                    break;

                case "Админ панель":
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WhatToAdd[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.ChoosingWhatToAdd(_language));
                    break;

                case "Место":
                    user.State = State.AddPlace;
                    user.StateAdd = StateAdd.GettingPlaceName;
                    await UserService.Update(user);

                    if (_newUserPlaces.ContainsKey(chatId))
                        _newUserPlaces[chatId] = new();
                    else
                        _newUserPlaces.Add(chatId, new());

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceName[(int)_language],
                        cancellationToken: cancellationToken);
                    break;

                case "Я все выбрал!":
                    user.StateAdd = StateAdd.End;
                    await UserService.Update(user);
                    goto default;

                default:
                    if(user.StateAdd != StateAdd.End)
                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Добавлено!");

                    switch (user.State)
                    {
                        case State.AddPlace:
                            await AddPlaceAsync(botClient, new Message() { Text = callbackQuery.Data, Chat = callbackQuery.Message.Chat }, cancellationToken, user);
                            break;
                    }
                    break;
            }
        }

        private static async Task AddPlaceAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;


            switch (user.StateAdd)
            {
                case StateAdd.GettingPlaceName:
                    if (await IsNotCorrectInput(userInput.Split(" ").Length == 2,userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceDescription[(int)_language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceDescription:
                    if (await IsNotCorrectInput(userInput.Split("\n\n").Length == 2, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Description = userInput;
                    user.StateAdd = StateAdd.GettingPlaceImage;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceImage[(int)_language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceImage:
                    var fileId = message.Photo.Last().FileId;
                    var destinationImagePath = _destinationFilePath + $"{_newUserPlaces[chatId].Name}Place.png";

                    await using (Stream fileStream = System.IO.File.Create(new string(destinationImagePath)))
                    {
                        var file = await botClient.GetInfoAndDownloadFileAsync(
                            fileId: fileId,
                            destination: fileStream,
                            cancellationToken: cancellationToken);
                    }

                    _newUserPlaces[chatId].Image = $"{_newUserPlaces[chatId].Name}Place.png";
                    user.StateAdd = StateAdd.GettingPlaceYId;

                    // Отправление фотографий
                    //using (var fileStream = new FileStream("Я путь", FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                    //    await botClient.SendPhotoAsync(
                    //        chatId: chatId,
                    //        photo: InputFile.FromStream(fileStream),
                    //        caption: $"Я текст",
                    //        cancellationToken: cancellationToken);
                    //}

                    await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.EnteringPlaceYId[(int)_language],
                       cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceYId:
                    if (await IsNotCorrectInput(userInput.Length == 12, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].YIdOrganization = long.Parse(userInput);
                    user.StateAdd = StateAdd.GettingPlaceCoordinates;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCoordinates[(int)_language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceCoordinates:
                    var listInput = userInput.Split(",");
                    if (await IsNotCorrectInput(listInput.Length == 2, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Coordinates = listInput[0].Trim() + "," + listInput[1].Trim();
                    user.StateAdd = StateAdd.GettingPlaceCategories;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_language, _newUserPlaces[chatId].Categories.Count != 0 ? _newUserPlaces[chatId].Categories.Last() : null));
                    break;

                case StateAdd.GettingPlaceCategories:
                    if (CategoryService.GetCategoryByName(userInput).Data == null)
                        break;

                    _newUserPlaces[chatId].Categories.Add(CategoryService.GetCategoryByName(userInput).Data);

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.Continue[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_language, _newUserPlaces[chatId].Categories.Last()));
                    break;

                case StateAdd.End:
                    await PlaceService.AddPlace(_newUserPlaces[chatId]);

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.PlaceSuccessfullyAdded[(int)_language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectAdministrator(_language));
                    break;
            };

            await UserService.Update(user);
        }

        private static async Task<bool> IsNotCorrectInput(bool condition, string userInput, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            if (condition) return false;

             await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.ErrorInput[(int)_language],
                        cancellationToken: cancellationToken);

             return true;
        }
    }
}