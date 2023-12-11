using YolaGuide.DAL;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.Domain.Enums;
using YolaGuide.Messages;
using YolaGuide.Domain.Entity;

namespace YolaGuide.Controllers
{
    public static class PlaceController
    {
        private static readonly PlaceService _placeService = new(new(new ApplicationDbContext(new())));
        private static readonly Dictionary<long, PlaceViewModel> _newUserPlaces = new();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserPlaces.ContainsKey(chatId))
                _newUserPlaces[chatId] = new();
            else
                _newUserPlaces.Add(chatId, new());
        }

        public static async Task CreatePlaceAsync(PlaceViewModel model)
        {
            var response = await _placeService.CreatePlace(model);

            if (response.StatusCode == StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task AddPlaceAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.GettingPlaceName:
                    if (await IsNotCorrectInput(userInput.Split("\n\n").Length == (int)Language.English + 1, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingPlaceDescription;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceDescription[(int)Settings.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceDescription:
                    if (await IsNotCorrectInput(userInput.Split("\n\n").Length == (int)Language.English + 1, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Description = userInput;
                    user.StateAdd = StateAdd.GettingPlaceAdress;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceAdress[(int)Settings.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceAdress:
                    _newUserPlaces[chatId].Adress = userInput;
                    user.StateAdd = StateAdd.GettingPlaceImage;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceImage[(int)Settings.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceImage:
                    var fileId = message.Photo.Last().FileId;
                    var destinationImagePath = Settings.DestinationImagePath + $"{_newUserPlaces[chatId].Name}Place.png";

                    await using (Stream fileStream = System.IO.File.Create(new string(destinationImagePath)))
                    {
                        var file = await botClient.GetInfoAndDownloadFileAsync(
                            fileId: fileId,
                            destination: fileStream,
                            cancellationToken: cancellationToken);
                    }

                    _newUserPlaces[chatId].Image = $"{_newUserPlaces[chatId].Name}Place.png";
                    user.StateAdd = StateAdd.GettingPlaceYId;

                    // Отправление фотографий
                    //using (var fileStream = new FileStream("Я путь", FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                    //    await botClient.SendPhotoAsync(
                    //        chatId: chatId,
                    //        photo: InputFile.FromStream(fileStream),
                    //        caption: $"Я текст",
                    //        cancellationToken: cancellationToken);
                    //}

                    await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: Answer.EnteringPlaceYId[(int)Settings.Language],
                       cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceYId:
                    if (await IsNotCorrectInput(userInput.Length == 12, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].YIdOrganization = long.Parse(userInput);
                    user.StateAdd = StateAdd.GettingPlaceCoordinates;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCoordinates[(int)Settings.Language],
                        cancellationToken: cancellationToken);
                    break;

                case StateAdd.GettingPlaceCoordinates:
                    var listInput = userInput.Split(",");
                    if (await IsNotCorrectInput(listInput.Length == 2, userInput, botClient, chatId, cancellationToken))
                        break;

                    _newUserPlaces[chatId].Coordinates = listInput[0].Trim() + "," + listInput[1].Trim();
                    user.StateAdd = StateAdd.GettingPlaceCategories;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_newUserPlaces[chatId].Categories.Count != 0 ? _newUserPlaces[chatId].Categories.Last() : null));
                    break;

                case StateAdd.GettingPlaceCategories:
                    if (CategoryController.GetCategoryByName(userInput) == null)
                        break;

                    _newUserPlaces[chatId].Categories.Add(CategoryController.GetCategoryByName(userInput));

                    await botClient.EditMessageTextAsync(
                        messageId: message.MessageId,
                        chatId: chatId,
                        text: Answer.EnteringPlaceCategories[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(_newUserPlaces[chatId].Categories.Last()));
                    break;

                case StateAdd.End:
                    await CreatePlaceAsync(_newUserPlaces[chatId]);
                    _newUserPlaces[chatId] = new();

                    user.State = State.Start;
                    user.StateAdd = StateAdd.Start;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.SuccessfullyAdded[(int)Settings.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.SelectAdministrator());
                    break;
            };

            await UserController.UpdateUser(user);
        }

        private static async Task<bool> IsNotCorrectInput(bool condition, string userInput, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            if (condition) return false;

            await botClient.SendTextMessageAsync(
            chatId: chatId,
                       text: Answer.ErrorInput[(int)Settings.Language],
                       cancellationToken: cancellationToken);

            return true;
        }
    }
}
