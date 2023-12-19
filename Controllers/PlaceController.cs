using YolaGuide.DAL;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.Domain.Enums;
using YolaGuide.Messages;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.Domain.Entity;
using Azure;

namespace YolaGuide.Controllers
{
    public static class PlaceController
    {
        private static readonly PlaceService _placeService = new(new PlaceRepository(new ApplicationDbContext()));
        private static readonly Dictionary<long, PlaceViewModel> _newUserPlaces = new();
        private static readonly Random _random = new();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserPlaces.ContainsKey(chatId))
                _newUserPlaces[chatId] = new();
            else
                _newUserPlaces.Add(chatId, new());
        }

        public static async Task CreateAsync(PlaceViewModel model)
        {
            var response = await _placeService.CreatePlace(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task RemoveAsync(Place place)
        {
            var response = await _placeService.RemovePlaceAsync(place);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static List<Place> GetPlacesByName(string name)
        {
            var response = _placeService.GetPlacesByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static List<Place> SearchPlace(string userInput)
        {
            var response = _placeService.Search(userInput);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static List<Place> GetPlacesByCategory(Category category)
        {
            var response = _placeService.GetPlacesByCategory(category);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static Place GetPlacesById(int id)
        {
            var response = _placeService.GetPlaceById(id);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static List<Place> GetAll()
        {
            var response = _placeService.GetAllPlace();

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static async Task AddPlaceAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceName;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringPlaceName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;
                case Substate.GettingPlaceName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].Name = userInput;
                    user.Substate = Substate.GettingPlaceDescription;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].Description = userInput;
                    user.Substate = Substate.GettingPlaceAdress;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceAdress[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceAdress:
                    _newUserPlaces[chatId].Adress = userInput;
                    user.Substate = Substate.GettingPlaceContactInformation;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceContactInformation[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceContactInformation:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].ContactInformation = userInput;
                    user.Substate = Substate.GettingPlaceYId;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceYId[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceYId:
                    _newUserPlaces[chatId].YIdOrganization = long.Parse(userInput);
                    user.Substate = Substate.GettingPlaceImage;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceImage[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceImage:
                    var fileId = message.Photo.Last().FileId;
                    var fileName = _newUserPlaces[chatId].YIdOrganization;
                    var destinationImagePath = Settings.DestinationImagePath + $"{fileName}Place.png";

                    await using (Stream fileStream = System.IO.File.Create(new string(destinationImagePath)))
                    {
                        var file = await botClient.GetInfoAndDownloadFileAsync(
                            fileId: fileId,
                            destination: fileStream,
                            cancellationToken: cancellationToken);
                    }

                    _newUserPlaces[chatId].Image = $"{fileName}Place.png";
                    user.Substate = Substate.GettingPlaceCoordinates;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.EnteringPlaceCoordinates[(int)user.Language],
                       cancellationToken: cancellationToken);
                    break;

                case Substate.GettingPlaceCoordinates:
                    var listInput = userInput.Split(",");

                    _newUserPlaces[chatId].Coordinates = listInput[0].Trim() + "," + listInput[1].Trim();
                    user.Substate = Substate.GettingPlaceCategories;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(null, user.Language));
                    break;

                case Substate.GettingPlaceCategories:
                    if (CategoryController.GetCategoryById(int.Parse(userInput)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);   

                        break;
                    }
                    var category = CategoryController.GetCategoryById(int.Parse(userInput));

                    if (_newUserPlaces[chatId].Categories.FirstOrDefault(c => c.Id == category.Id) == null)
                        _newUserPlaces[chatId].Categories.Add(category);

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_newUserPlaces[chatId].Categories.Last(), user.Language));
                    break;

                case Substate.End:
                    await CreateAsync(_newUserPlaces[chatId]);
                    _newUserPlaces[chatId] = new();

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SuccessfullyAdded(user.Language));
                    break;
            };

            await UserController.UpdateUser(user);
        }

        public static async Task AddPlaceInPlanAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlanCategory;

                    if (message.Text == "Назад" || message.Text == "Добавить\nplan")
                        goto case Substate.GettingPlanCategory;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: Answer.MakingPlan[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.PlanCategorySelection(null, user.Language));
                    break;

                case Substate.GettingPlanCategory:
                    Category category = null;

                    if (int.TryParse(message.Text, out int id))
                        category = CategoryController.GetCategoryById(id);

                    if (category == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: Answer.MakingPlan[(int)user.Language],
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.PlanCategorySelection(null, user.Language));

                        break;
                    }

                    if (category.Subcategories.Count == 0)
                    {
                        user.Substate = Substate.GettingPlanPlaceName;

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanPlace[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetNamePlaceByCategory(category, user.Language, 1));
                        break;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.MakingPlan[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.PlanCategorySelection(category, user.Language));
                    break;

                case Substate.GettingPlanPlaceName:
                    if(GetPlacesByName(message.Text) != null)
                    {
                        user.Substate = Substate.End;

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanAdress[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceAddressesByName(message.Text, user.Language));

                        break;
                    }

                    var categoryPageNumber = message.Text.Split(" ").ToList();
                    category = CategoryController.GetCategoryByName(categoryPageNumber[0]);

                    if(category == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    if (categoryPageNumber.Count != 2) categoryPageNumber.Add("1");

                    if (int.TryParse(categoryPageNumber[1], out int page) && page > 0)
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanPlace[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetNamePlaceByCategory(category, user.Language, page));
                    break;

                case Substate.End:
                    var place = GetPlacesById(int.Parse(message.Text));

                    if (place == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task GetPlan(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceInPlan;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: Answer.GetPlanInformation(user),
                                cancellationToken: cancellationToken,
                                replyMarkup: Keyboard.GetPlanCard(user.Language, user));
                    break;

                case Substate.GettingPlaceInPlan:
                    user.Substate = Substate.End;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.PlacesInPlan[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetPlaceInList(user.Language, user.Places));
                    break;

                case Substate.End:
                    var place = GetPlacesById(int.Parse(message.Text));

                    if (place == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;

            }

            await UserController.UpdateUser(user);
        }

        public static async Task DeletePlaceInPlan(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceInPlanToDelete;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.PlacesInPlan[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetPlaceInList(user.Language, user.Places));
                    break;

                case Substate.GettingPlaceInPlanToDelete:
                    var place = GetPlacesById(int.Parse(message.Text));

                    if (place == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    place = user.Places.FirstOrDefault(p => p.Id == place.Id);

                    user.Places.Remove(place);
                    await UserController.UpdateUser(user);

                    user.Substate = Substate.Start;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.PlacesInPlan[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetPlaceInList(user.Language, user.Places));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task Search(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceNameSearch;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: Answer.EnteringStringToSearch[(int)user.Language],
                            cancellationToken: cancellationToken);
                    break;
                case Substate.GettingPlaceNameSearch:
                    var userInput = message.Text;

                    if (userInput == "Назад")
                    {
                        await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                        goto case Substate.Start;
                    }

                    user.Substate = Substate.GettingPlaceAdressSearch;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlanPlace[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetResultSearch(user.Language, userInput, 1));
                    break;

                case Substate.GettingPlaceAdressSearch:
                    var places = SearchPlace(message.Text);
                    if (places.Count != 0)
                    {
                        user.Substate = Substate.End;

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanAdress[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceAddressesByName(places[0].Name, user.Language));
                        break;
                    }

                    var userInputPageNumber = message.Text.Split(" ").ToList();

                    if (userInputPageNumber.Count != 2) userInputPageNumber.Add("1");

                    if (int.TryParse(userInputPageNumber[1], out int page) && page > 0)
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanPlace[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetResultSearch(user.Language, userInputPageNumber[0], page));
                    break;
                case Substate.End:
                    var place = GetPlacesById(int.Parse(message.Text));

                    if (GetPlacesById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await botClient.AnswerCallbackQueryAsync(message.MessageId.ToString(), Answer.Added[(int)user.Language]);

                    user.Places.Add(place);
                    user.State = State.Start;
                    user.Substate = Substate.Start;
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task DeletePlaceAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.Start:
                    user.Substate = Substate.GettingPlaceToDelete;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.DeletePlace[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetAllPlace(user.Language, 1));
                    break;

                case Substate.GettingPlaceToDelete:
                    if (GetPlacesByName(message.Text) != null)
                    {
                        user.Substate = Substate.End;

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
                            text: Answer.DeletePlace[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetAllPlace(user.Language, page));
                    break;

                case Substate.End:
                    if (GetPlacesById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    await RemoveAsync(GetPlacesById(int.Parse(message.Text)));

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SuccessfullyDellete[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.Back(user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static Place GetRandomPlace(Domain.Entity.User user)
        {
            var places = GetAll();

            if (places.Count == 0)
                return null;

            if (user.Categories.Count != 0)
                places = GetPlacesByCategory(user.Categories[_random.Next(user.Categories.Count)]);

            if (places.Count != 0)
                return places[_random.Next(places.Count)];
            else
                return null;
        }

        public async static Task GetPlaceCard(ITelegramBotClient botClient, CancellationToken cancellationToken, Place place, Domain.Entity.User user, Message message)
        {
            switch (user.Substate)
            {
                case Substate.End:
                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    using (var fileStream = new FileStream(Settings.DestinationImagePath + place.Image, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Settings.LastBotMsg[user.Id] = await botClient.SendPhotoAsync(
                            chatId: user.Id,
                            photo: InputFile.FromStream(fileStream),
                            caption: Answer.GetPlaceInformation(place, user.Language),
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetPlaceCard(user.Language, place, user));
                    }
                    break;

                case Substate.GettingPlaceInformation:
                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                            chatId: user.Id,
                            text: place.ContactInformation.Split("\n\n\n")[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.Back(user.Language, place.Id.ToString()));
                    break;

                case Substate.GettingSimilarPlaces:
                    user.Substate = Substate.GettingPlaceNavigation;

                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                            chatId: user.Id,
                            text: Answer.GetSimilarPlaces[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GettingPlacesInListAndButtonNavigation(user.Language, 1, GetPlacesByCategory(place.Categories.Last()), place.Id.ToString()));
                    break;

                case Substate.GettingPlacesNearby:
                    user.Substate = Substate.GettingPlaceNavigation;

                    var places = GetAll().ToList();
                    List<Place> nearbyPlaces = new();

                    foreach (var p in places)
                        if (CalculateDistanceInMeters(p.Coordinates, place.Coordinates) < 100)
                            nearbyPlaces.Add(p);

                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                            chatId: user.Id,
                            text: Answer.GetNearbyPlaces[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GettingPlacesInListAndButtonNavigation(user.Language, 1, nearbyPlaces, place.Id.ToString()));
                    break;

                case Substate.GettingPlaceNavigation:
                    if (GetPlacesById(int.Parse(message.Text)) != null)
                        goto case Substate.Start;

                    var listIdAndPageNumber = message.Text.Split("\n").ToList();
                    places = listIdAndPageNumber[0].Split(" ").Select(id => GetPlacesById(int.Parse(id))).ToList();

                    if (listIdAndPageNumber.Count != 2) listIdAndPageNumber.Add("1");

                    if (int.TryParse(listIdAndPageNumber[1], out int page) && page > 0)                
                        Settings.LastBotMsg[user.Id] = await botClient.EditMessageTextAsync(
                                messageId: Settings.LastBotMsg[user.Id].MessageId,
                                chatId: user.Id,
                                text: Answer.GetSimilarPlaces[(int)user.Language],
                                cancellationToken: cancellationToken,
                                replyMarkup: Keyboard.GettingPlacesInListAndButtonNavigation(user.Language, page, places, place.Id.ToString()));
                    break;

                case Substate.GettingPlaceRoutes:
                    user.Substate = Substate.GettingPlaceRoutesNavigation;

                    await botClient.DeleteMessageAsync(
                        chatId: user.Id,
                        messageId: Settings.LastBotMsg[user.Id].MessageId);

                    Settings.LastBotMsg[user.Id] = await botClient.SendTextMessageAsync(
                            chatId: user.Id,
                            text: Answer.GettingRoute[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetRoutesByPlace(user.Language, 1, place, place.Id.ToString()));
                    break;

                case Substate.GettingPlaceRoutesNavigation:
                    if (RouteController.GetByName(message.Text) != null)
                        goto case Substate.Start;

                    var userInputPageNumber = message.Text.Split(" ").ToList();

                    if (userInputPageNumber[0] != $"{place.Id}")
                        goto case Substate.GettingPlaceRoutes;


                    if (userInputPageNumber.Count != 2) userInputPageNumber.Add("1");

                    if (int.TryParse(userInputPageNumber[1], out page) && page > 0)
                        Settings.LastBotMsg[user.Id] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[user.Id].MessageId,
                            chatId: user.Id,
                            text: Answer.GettingRoute[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetRoutesByPlace(user.Language, page, place, place.Id.ToString()));
                    break;

                case Substate.Start:
                    var route = RouteController.GetByName(message.Text); 
                    
                    if (route != null) 
                    {
                        await RouteController.GetRouteCard(botClient, cancellationToken, route, user);
                        break;
                    }

                    place = GetPlacesById(int.Parse(message.Text));

                    user.Substate = Substate.End;
                    await UserController.UpdateUser(user);

                    await GetPlaceCard(botClient, cancellationToken, place, user, message);
                    break;
            }
        }

        private static double CalculateDistanceInMeters(string firstCoordinats, string secondCoordinats)
        {
            var x1 = double.Parse(firstCoordinats.Split(",")[0], System.Globalization.CultureInfo.InvariantCulture);
            var y1 = double.Parse(firstCoordinats.Split(",")[1], System.Globalization.CultureInfo.InvariantCulture);

            var x2 = double.Parse(secondCoordinats.Split(",")[0], System.Globalization.CultureInfo.InvariantCulture);
            var y2 = double.Parse(secondCoordinats.Split(",")[1], System.Globalization.CultureInfo.InvariantCulture);

            var cosinusX1 = Math.Cos(x1);
            var cosinusX2 = Math.Cos(x2);

            var sinusX1 = Math.Sin(x1);
            var sinusX2 = Math.Sin(x2);

            var delta = y2 - y1;

            var cosinusDelta = Math.Cos(delta);
            var sinusDelta = Math.Sin(delta);

            var y = Math.Sqrt(Math.Pow(cosinusX2 * sinusDelta, 2) + Math.Pow(cosinusX1 * sinusX2 - sinusX1 * cosinusX2 * cosinusDelta, 2));
            var x = sinusX1 * sinusX2 + cosinusX1 * cosinusX2 * cosinusDelta;

            var ad = Math.Atan2(y, x);
            return ad * 6372795;
        }
    }
}
