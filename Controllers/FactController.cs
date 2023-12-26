using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Messages;
using YolaGuide.Domain.Entity;
using YolaGuide.Service.Interfaces;
using Telegram.Bot.Types.Enums;

namespace YolaGuide.Controllers
{
    public class FactController
    {
        private readonly IFactService _factService;
        private readonly Dictionary<long, FactViewModel> _newUserFact = new();
        private readonly Random _random = new();

        public FactController(IFactService factService)
        {
            _factService = factService;
        }

        public void AddNewPairInDictionary(long chatId)
        {
            if (_newUserFact.ContainsKey(chatId))
                _newUserFact[chatId] = new();
            else
                _newUserFact.Add(chatId, new());
        }

        public async Task CreateAsync(FactViewModel model)
        {
            var response = await _factService.CreateFactAsync(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public async Task RemoveAsync(Fact fact)
        {
            var response = await _factService.RemoveFactAsync(fact);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public List<Fact> GetAll()
        {
            var response = _factService.GetAllFact();

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public Fact GetFactByName(string name)
        {
            var response = _factService.GetFactByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public async Task AddFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
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
                        parseMode: ParseMode.Html,
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
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingFactDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserFact[chatId].Description = userInput;
                    user.Substate = Substate.End;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringFactImage[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                    break;

                case Substate.End:
                    var fileId = message.Photo.Last().FileId;
                    var fileName = _newUserFact[chatId].Name.Split("\n\n\n")[0].Replace( " ", "").Replace("-", "").Replace("\"", "");
                    var destinationImagePath = Settings.DestinationImagePath + $"{fileName}Fact.png";

                    await using (Stream fileStream = System.IO.File.Create(new string(destinationImagePath)))
                    {
                        var file = await botClient.GetInfoAndDownloadFileAsync(
                            fileId: fileId,
                            destination: fileStream,
                            cancellationToken: cancellationToken);
                    }

                    _newUserFact[chatId].Image = $"{fileName}Fact.png";

                    await CreateAsync(_newUserFact[chatId]);

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SuccessfullyAdded(user.Language));
                    break;
            }

            await Settings.UserController.UpdateUser(user);
        }

        public async Task DeleteFactAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
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
                        parseMode: ParseMode.Html,
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
                        parseMode: ParseMode.Html,
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
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllFacts(user.Language, page));
                    break;
            }
        }

        public Fact GetRandomFact()
        {
            var facts = GetAll();

            if (facts.Count == 0)
                return null;

            return facts[_random.Next(facts.Count)];
        }
    }
}
