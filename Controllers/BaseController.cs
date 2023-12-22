using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using YolaGuide.Domain.Enums;
using YolaGuide.Messages;

namespace YolaGuide.Controllers
{
    internal static class BaseController
    {
        internal static async Task<bool> IsNotCorrectInput(string userInput, ITelegramBotClient botClient, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            if (userInput.Split("\n\n\n").Length == (int)Language.English + 1) return false;

            Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                chatId: user.Id,
                text: Answer.WrongInputFormat[(int)user.Language],
                cancellationToken: cancellationToken);

            return true;
        }

        internal static async Task ShowError(ITelegramBotClient botClient, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                chatId: user.Id,
                text: Answer.Error[(int)user.Language],
                cancellationToken: cancellationToken);
        }

        internal static async Task EditBackMessage(ITelegramBotClient botClient, CancellationToken cancellationToken, Domain.Entity.User user, InlineKeyboardMarkup keyboard, string message)
        {
            Settings.LastBotMsg[user.Id] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[user.Id].MessageId,
                            chatId: user.Id,
                            text: message,
                            cancellationToken: cancellationToken,
                            replyMarkup: keyboard);
        }


    }
}
