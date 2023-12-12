using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using YolaGuide.Messages;

namespace YolaGuide.Controllers
{
    public class FactController
    {
        private static readonly FactService _factService = new(new FactReposutory(new ApplicationDbContext(new())));
        private static readonly Dictionary<long, FactViewModel> _newUserFact = new();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserFact.ContainsKey(chatId))
                _newUserFact[chatId] = new();
            else
                _newUserFact.Add(chatId, new());
        }

        public static async Task CreatePlaceAsync(FactViewModel model)
        {
            var response = await _factService.CreateFact(model);

            if (response.StatusCode == StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task AddFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.GettingFactName:
                    if (await IsNotCorrectInput(userInput, botClient, chatId, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringFactDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingFactSDescription:
                    if (await IsNotCorrectInput(userInput, botClient, chatId, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Description = userInput;
                    user.StateAdd = StateAdd.Start;

                    await CreatePlaceAsync(_newUserFact[chatId]);
                    _newUserFact[chatId] = new();

                    user.State = State.Start;
                    user.StateAdd = StateAdd.Start;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        private static async Task<bool> IsNotCorrectInput(string userInput, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            if (userInput.Split("\n\n").Length == (int)Language.English + 1) return false;

            Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
            chatId: chatId,
                       text: Answer.WrongInputFormat[(int)user.Language],
                       cancellationToken: cancellationToken);

            return true;
        }
    }
}
