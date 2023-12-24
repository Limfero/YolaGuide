using YolaGuide.Domain.ViewModel;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.Messages;
using Telegram.Bot.Types.ReplyMarkups;
using YolaGuide.Service.Interfaces;

namespace YolaGuide.Controllers
{
    public class RouteController
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        private readonly Dictionary<long, RouteViewModel> _newUserRoute = new();

        public void AddNewPairInDictionary(long chatId)
        {
            if (_newUserRoute.ContainsKey(chatId))
                _newUserRoute[chatId] = new();
            else
                _newUserRoute.Add(chatId, new());
        }

        public async Task CreateAsync(RouteViewModel model)
        {
            var response = await _routeService.CreateRouteAsync(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public async Task RemoveAsync(Route route)
        {
            var response = await _routeService.RemoveRouteAsync(route);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public List<Route> GetAll()
        {
            var response = _routeService.GetAllRoutes();

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public Route GetByName(string name)
        {
            var response = _routeService.GetRouteByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public Route GetById(int id)
        {
            var response = _routeService.GetRouteById(id);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public async Task AddRouteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingRouteName;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingRouteName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    user.Substate = Substate.GettingRouteDescription;

                    _newUserRoute[chatId].Name = userInput;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingRouteDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    user.Substate = Substate.GettingRouteCost;

                    _newUserRoute[chatId].Description = userInput;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteCost[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingRouteCost:
                    if (!decimal.TryParse(userInput, out decimal cost))
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.WrongInputFormat[(int)user.Language],
                        cancellationToken: cancellationToken);

                        break;
                    }

                    user.Substate = Substate.GettingRouteTelephone;

                    _newUserRoute[chatId].Cost = cost;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteTelephone[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingRouteTelephone:
                    user.Substate = Substate.GettingRoutePlaces;

                    _newUserRoute[chatId].Telephone = userInput;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRoutePlaces[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetAllPlacesWithoutRoute(user.Language, 1, _newUserRoute[chatId].Places));
                    break;

                case Substate.GettingRoutePlaces:
                    if (Settings.PlaceController.GetPlacesByName(message.Text).Count != 0)
                    {
                        user.Substate = Substate.GettingRoutePlaceAdress;

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanAdress[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceAddressesByName(message.Text, user.Language));
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
                            text: Answer.EnteringRoutePlaces[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllPlacesWithoutRoute(user.Language, page, _newUserRoute[chatId].Places));
                    break;

                case Substate.GettingRoutePlaceAdress:
                    var place = Settings.PlaceController.GetPlaceById(int.Parse(message.Text));

                    if (place == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    _newUserRoute[chatId].Places.Add(place);

                    user.Substate = Substate.GettingRoutePlaces;

                    var keyboard = Keyboard.GetAllPlacesWithoutRoute(user.Language, 1, _newUserRoute[chatId].Places);

                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)user.Language], "Я все выбрал!"),
                    }));

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringRoutePlaces[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: keyboard);
                    break;

                case Substate.End:
                    await CreateAsync(_newUserRoute[chatId]);
                    _newUserRoute[chatId].Places = new();

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SuccessfullyAdded(user.Language));
                    break;
            }

            await Settings.UserController.UpdateUser(user);
        }

        public async Task DeleteRouteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingRouteToDelete;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.DeleteFact[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetAllRoutes(user.Language, 1));
                    break;

                case Substate.GettingRouteToDelete:
                    var route = GetByName(message.Text);

                    if (route != null)
                    {
                        await RemoveAsync(route);

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
                            replyMarkup: Keyboard.GetAllRoutes(user.Language, page));
                    break;
            }
        }

        public async Task GetAllRoutes(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingAllRoute;

                    if (message.Text.Contains("Назад"))
                    {
                        await botClient.DeleteMessageAsync(
                            chatId: user.Id,
                            messageId: Settings.LastBotMsg[user.Id].MessageId);
                    }

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: Answer.GettingRoute[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllRoutes(user.Language, 1));
                    break;

                case Substate.GettingAllRoute:
                    var route = GetByName(message.Text);

                    if (route != null)
                    {
                        await GetRouteCard(botClient, cancellationToken, route, user);

                        return;
                    }

                    var userInputPageNumber = message.Text.Split(" ").ToList();

                    if (userInputPageNumber[0] != "all")
                        goto case Substate.GettingAllPlaceInRoute;


                    if (userInputPageNumber.Count != 2) userInputPageNumber.Add("1");

                    if (int.TryParse(userInputPageNumber[1], out int page) && page > 0)
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.GettingRoute[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllRoutes(user.Language, page));
                    break;

                case Substate.GettingAllPlaceInRoute:
                    user.Substate = Substate.End;

                    var idPlaces = message.Text.Split(" ");

                    List<Place> places = new();

                    foreach (var id in idPlaces)
                        places.Add(Settings.PlaceController.GetPlaceById(int.Parse(id)));

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.PlacesInRoute[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceInList(user.Language, places));
                    break;

                case Substate.End:
                    var place = Settings.PlaceController.GetPlaceById(int.Parse(message.Text));

                    if (Settings.PlaceController.GetPlaceById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await Settings.PlaceController.GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;

            }

            await Settings.UserController.UpdateUser(user);
        }

        public async Task GetRouteCard(ITelegramBotClient botClient, CancellationToken cancellationToken, Route route, Domain.Entity.User user)
        {
            Settings.LastBotMsg[user.Id] = await botClient.EditMessageTextAsync(
                       messageId: Settings.LastBotMsg[user.Id].MessageId,
                       chatId: user.Id,
                       text: Answer.GetRouteInformation(route, user.Language),
                       cancellationToken: cancellationToken,
                       replyMarkup: Keyboard.GetRouteCard(user.Language, route));
        }
    }
}
