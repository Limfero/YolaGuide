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
        private static readonly FactService _factService = new(new FactReposutory(new ApplicationDbContext()));
        private static readonly Dictionary<long, FactViewModel> _newUserFact = new();
        private static readonly Random _random = new Random();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserFact.ContainsKey(chatId))
                _newUserFact[chatId] = new();
            else
                _newUserFact.Add(chatId, new());
        }

        public static async Task CreateAsync(FactViewModel model)
        {
            var response = await _factService.CreateFactAsync(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task RemoveAsync(Fact fact)
        {
            var response = await _factService.RemoveFactAsync(fact);

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

        public static Fact GetFactByName(string name)
        {
            var response = _factService.GetFactByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static async Task AddFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingFactName;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringFactName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingFactName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Name = userInput;
                    user.Substate = Substate.GettingFactDescription;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringFactDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingFactDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Description = userInput;
                    user.Substate = Substate.Start;

                    await CreateAsync(_newUserFact[chatId]);
                    _newUserFact[chatId] = new();

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SuccessfullyAdded(user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task DeleteFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingFactToDelete;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.DeleteFact[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetAllFacts(user.Language, 1));
                    break;

                case Substate.GettingFactToDelete:
                    var fact = GetFactByName(message.Text);

                    if (fact != null)
                    {
                        await RemoveAsync(fact);

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SuccessfullyDellete[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.Back(user.Language));

                        break;
                    }

                    var userInputPageNumber = message.Text.Split(" ").ToList();

                    if (userInputPageNumber[0] != "all")
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    if (userInputPageNumber.Count != 2) userInputPageNumber.Add("1");

                    if (int.TryParse(userInputPageNumber[1], out int page) && page > 0)
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.DeleteFact[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllFacts(user.Language, page));
                    break;
            }
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
