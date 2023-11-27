using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace YolaGuide
{
    public class Program
    {
        private static readonly string _token = "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4";
        private static ITelegramBotClient? _botClient;

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

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cancellationTokenSource.Cancel();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            switch (messageText.ToLower())
            {
                case "/start":
                    await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Выбоооооор",
                       cancellationToken: cancellationToken,
                       replyMarkup: GetKeyboard());
                    break;
                default:
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "К сожалению, я ещё тупенький и не знаю этой команды",
                        cancellationToken: cancellationToken);
                    break;
            }

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
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

        private static InlineKeyboardMarkup GetKeyboard()
        {
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>() 
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Ссылка на ПГТУ!", "https://yandex.ru/maps/org/203257135115"),
                    InlineKeyboardButton.WithCallbackData("А это просто кнопка", "button1"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Тут еще одна", "button2"),
                    InlineKeyboardButton.WithCallbackData("И здесь", "button3"),
                },
            });
        }
    }
}