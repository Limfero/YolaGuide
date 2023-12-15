using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using YolaGuide.Messages;
using YolaGuide.Domain.Entity;

namespace YolaGuide.Controllers
{
    public class FactController
    {
        private static readonly FactService _factService = new(new FactReposutory(new ApplicationDbContext(new())));
        private static readonly Dictionary<long, FactViewModel> _newUserFact = new();
        private static readonly Random _random = new Random();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserFact.ContainsKey(chatId))
                _newUserFact[chatId] = new();
            else
                _newUserFact.Add(chatId, new());
        }

        public static async Task CreateFactAsync(FactViewModel model)
        {
            var response = await _factService.CreateFact(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static List<Fact> GetAll()
        {
            var response = _factService.GetAllFact();

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static async Task AddFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.StartAddPlan:
                    user.StateAdd = StateAdd.GettingFactName;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringFactName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingFactName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringFactDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingFactDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Description = userInput;
                    user.StateAdd = StateAdd.Start;

                    await CreateFactAsync(_newUserFact[chatId]);
                    _newUserFact[chatId] = new();

                    user.State = State.Start;
                    user.StateAdd = StateAdd.Start;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static Fact GetRandomFact()
        {
            var facts = GetAll();

            if (facts.Count == 0)
                return null;

            return facts[_random.Next(facts.Count)];
        }
    }
}
