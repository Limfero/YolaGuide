using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YolaGuide.Settings;
using YolaGuide.Domain.Enums;

namespace YolaGuide
{
    public class Program
    {
        private static readonly string _token = "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4";
        private static ITelegramBotClient? _botClient;
        private static Language _language = Language.Russian;

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
                        var chatId = message.Chat.Id;
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                await ReplyToTextMessage(botClient, message, cancellationToken);
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
                        await ReplyToCallbackQuery(botClient, update, cancellationToken);
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

        private static async Task ReplyToTextMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
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
                       replyMarkup: Keyboard.LanguageChangeKeyboard);

                    return;
                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "К сожалению, я ещё тупенький и не знаю этой команды",
                        cancellationToken: cancellationToken);
                    return;
            }
        }

        private static async Task ReplyToCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var callbackQuery = update.CallbackQuery;
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);

            switch (callbackQuery.Data)
            {
                case "English":
                case "Русский":
                    if (callbackQuery.Data == "English")
                        _language = Language.English;
                    else
                        _language = Language.Russian;

                    await SendWelcomeMessage(botClient, callbackQuery.Message.Chat.Id, _language, cancellationToken);
                    return;
            }
            return;
        }

        private static async Task SendWelcomeMessage(ITelegramBotClient botClient, long chatId, Language language, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WelcomeMessage[(int)language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStartKeyboard(language));
        }
    }
}