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
        private static readonly PlaceService _placeService = new(new PlaceRepository(new ApplicationDbContext(new())));
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

        public static List<Place> GetPlacesByName(string name)
        {
            var response = _placeService.GetPlacesByName(name);

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

            switch (user.StateAdd)
            {
                case StateAdd.StartAddPlace:
                    user.StateAdd = StateAdd.GettingPlaceName;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringPlaceName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;
                case StateAdd.GettingPlaceName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceDescription[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceDescription:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].Description = userInput;
                    user.StateAdd = StateAdd.GettingPlaceAdress;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceAdress[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceAdress:
                    _newUserPlaces[chatId].Adress = userInput;
                    user.StateAdd = StateAdd.GettingPlaceContactInformation;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceContactInformation[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceContactInformation:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserPlaces[chatId].ContactInformation = userInput;
                    user.StateAdd = StateAdd.GettingPlaceImage;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceImage[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceImage:
                    var fileId = message.Photo.Last().FileId;
                    var fileName = _newUserPlaces[chatId].Name.Split("\n\n")[(int)Language.Russian] + _newUserPlaces[chatId].Adress;
                    var destinationImagePath = Settings.DestinationImagePath + $"{fileName}Place.png";

                    await using (Stream fileStream = System.IO.File.Create(new string(destinationImagePath)))
                    {
                        var file = await botClient.GetInfoAndDownloadFileAsync(
                            fileId: fileId,
                            destination: fileStream,
                            cancellationToken: cancellationToken);
                    }

                    _newUserPlaces[chatId].Image = $"{_newUserPlaces[chatId].Name}Place.png";
                    user.StateAdd = StateAdd.GettingPlaceYId;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.EnteringPlaceYId[(int)user.Language],
                       cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceYId:
                    _newUserPlaces[chatId].YIdOrganization = long.Parse(userInput);
                    user.StateAdd = StateAdd.GettingPlaceCoordinates;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCoordinates[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceCoordinates:
                    var listInput = userInput.Split(",");

                    _newUserPlaces[chatId].Coordinates = listInput[0].Trim() + "," + listInput[1].Trim();
                    user.StateAdd = StateAdd.GettingPlaceCategories;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_newUserPlaces[chatId].Categories.Count != 0 ? _newUserPlaces[chatId].Categories.Last() : null, user.Language));
                    break;

                case StateAdd.GettingPlaceCategories:
                    if (CategoryController.GetCategoryByName(userInput) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);   

                        break;
                    }

                    _newUserPlaces[chatId].Categories.Add(CategoryController.GetCategoryByName(userInput));

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_newUserPlaces[chatId].Categories.Last(), user.Language));
                    break;

                case StateAdd.End:
                    await CreateAsync(_newUserPlaces[chatId]);
                    _newUserPlaces[chatId] = new();

                    user.State = State.Start;
                    user.StateAdd = StateAdd.Start;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.GetStart(chatId, user.Language));
                    break;
            };

            await UserController.UpdateUser(user);
        }

        public static async Task AddPlaceInPlanAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.StartAddPlan:
                    user.StateAdd = StateAdd.GettingPlanCategory;

                    if (user.Categories.Count == 0)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: Answer.NoPlanForToday[(int)user.Language],
                                cancellationToken: cancellationToken,
                                replyMarkup: Keyboard.PlanCategorySelection(null, user.Language));

                        break;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: Answer.WhatToAdd[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.PlanCategorySelection(null, user.Language));
                    break;

                case StateAdd.GettingPlanCategory:
                    var category = CategoryController.GetCategoryByName(message.Text);

                    if (CategoryController.GetCategoryByName(message.Text) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    if (category.Subcategories.Count == 0)
                    {
                        user.StateAdd = StateAdd.GettingPlanAdress;


                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanCategory[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetNamePlaceByCategory(category, user.Language, 1));
                        break;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.NoPlanForToday[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.PlanCategorySelection(category, user.Language));
                    break;

                case StateAdd.GettingPlanAdress:
                    if(GetPlacesByName(message.Text) != null)
                    {
                        user.StateAdd = StateAdd.GettingPlanPlace;

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
                    {
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringPlanCategory[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.GetNamePlaceByCategory(category, user.Language, page));
                    }
                    break;

                case StateAdd.GettingPlanPlace:
                    var place = GetPlacesById(int.Parse(message.Text));

                    if (GetPlacesById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    user.StateAdd = StateAdd.End;

                    await GetPlaceCard(botClient, cancellationToken, place, user);
                    break;

                case StateAdd.End:
                    place = GetPlacesById(int.Parse(message.Text));

                    if (GetPlacesById(int.Parse(message.Text)) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    user.Places.Add(place);
                    user.State = State.Start;
                    user.StateAdd = StateAdd.Start;

                    await GetPlaceCard(botClient, cancellationToken, place, user);
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

            return places[_random.Next(places.Count)];
        }

        public async static Task GetPlaceCard(ITelegramBotClient botClient, CancellationToken cancellationToken, Place place, Domain.Entity.User user)
        {
            using (var fileStream = new FileStream(Settings.DestinationImagePath + place.Image, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await botClient.SendPhotoAsync(
                    chatId: user.Id,
                    photo: InputFile.FromStream(fileStream),
                    caption: Answer.GetPlaceInformation(place, user.Language),
                    cancellationToken: cancellationToken,
                    replyMarkup: Keyboard.GetPlaceCard(user.Language, place));
            }
        }

        public static async Task Search(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.StartSearch:
                    user.StateAdd = StateAdd.GettingStringToSearch;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.EnteringStringToSearch[(int)user.Language],
                            cancellationToken: cancellationToken);
                    break;
                case StateAdd.GettingStringToSearch:


                    break;
            }

            await UserController.UpdateUser(user);
        }
    }
}
