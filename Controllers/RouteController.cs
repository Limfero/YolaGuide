using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.Messages;
using Telegram.Bot.Types.ReplyMarkups;

namespace YolaGuide.Controllers
{
    public class RouteController
    {
        private static readonly RouteService _routeService = new(new RouteRepository(new ApplicationDbContext()));
        private static readonly Dictionary<long, RouteViewModel> _newUserRoute = new();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserRoute.ContainsKey(chatId))
                _newUserRoute[chatId] = new();
            else
                _newUserRoute.Add(chatId, new());
        }

        public static async Task CreateAsync(RouteViewModel model)
        {
            var response = await _routeService.CreateRouteAsync(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task RemoveAsync(Route route)
        {
            var response = await _routeService.RemoveRouteAsync(route);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static List<Route> GetAll()
        {
            var response = _routeService.GetAllRoutes();

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static Route GetByName(string name)
        {
            var response = _routeService.GetRouteByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static async Task AddRouteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
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
                    user.Substate = Substate.GettingRouteDescription;

                    _newUserRoute[chatId].Name = userInput;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingRouteDescription:
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

                    _newUserRoute[chatId].Description = userInput;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteCost[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetAllPlace(user.Language, 1));
                    break;

                case Substate.GettingRoutePlaces:
                    if (PlaceController.GetPlacesByName(message.Text) != null)
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
                            text: Answer.EnteringPlanPlace[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllPlace(user.Language, page));
                    break;

                case Substate.GettingRoutePlaceAdress:
                    var place = PlaceController.GetPlacesById(int.Parse(message.Text));

                    if (place == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    _newUserRoute[chatId].Places.Add(place);

                    user.Substate = Substate.GettingRoutePlaces;

                    var keyboard = Keyboard.GetAllPlace(user.Language, 1);

                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)user.Language], "Я все выбрал!"),
                    }));

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringRouteCost[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: keyboard);
                    break;

                case Substate.End:
                    await CreateAsync(_newUserRoute[chatId]);
                    _newUserRoute[chatId] = new();

                    user.State = State.Start;
                    user.Substate = Substate.Start;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task DeleteRouteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceToDelete;

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

                        user.State = State.Start;
                        user.Substate = Substate.Start;

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

        public static async Task GetAllRoutes(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingAllRoute;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.DeleteFact[(int)user.Language],
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
                    {
                        user.Substate = Substate.GettingAllPlaceInRoute;

                        await BaseController.ShowError(botClient, cancellationToken, user);

                        return;
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

                case Substate.GettingAllPlaceInRoute:
                    var idPlaces = message.Text.Split(" ");

                    List<Place> places = new();

                    foreach (var id in idPlaces)
                        places.Add(PlaceController.GetPlacesById(int.Parse(id)));

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.DeleteFact[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceInList(user.Language, places));
                    break;

                case Substate.End:
                    var place = PlaceController.GetPlacesById(int.Parse(message.Text));

                    if (PlaceController.GetPlacesById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await PlaceController.GetPlaceCard(botClient, cancellationToken, place, user);
                    break;

            }

            await UserController.UpdateUser(user);
        }

        private static async Task GetRouteCard(ITelegramBotClient botClient, CancellationToken cancellationToken, Route route, Domain.Entity.User user)
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
