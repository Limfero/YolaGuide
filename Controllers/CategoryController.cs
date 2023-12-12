﻿using Telegram.Bot.Types;
using Telegram.Bot;
using YolaGuide.DAL;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;
using YolaGuide.Domain.Enums;
using YolaGuide.Messages;
using YolaGuide.DAL.Repositories.Implimentation;

namespace YolaGuide.Controllers
{
    public static class CategoryController
    {
        private static readonly CategoryService _categoryService = new(new CategoryRepository(new ApplicationDbContext(new())));
        private static readonly Dictionary<long, CategoryViewModel> _newUserCategory = new();

        public static void AddNewPairInDictionary(long chatId)
        {
            if (_newUserCategory.ContainsKey(chatId))
                _newUserCategory[chatId] = new();
            else
                _newUserCategory.Add(chatId, new());
        }

        public static Category GetCategoryByName(string name) 
        { 
            var response = _categoryService.GetCategoryByName(name);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static List<Category> GetCategories(Category category)
        {
            var response = _categoryService.GetCategores(category);

            if (response.StatusCode == StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static async Task CreateCategotyAsync(CategoryViewModel model)
        {
            var response = await _categoryService.CreateCategory(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task AddCategoryAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.StateAdd)
            {
                case StateAdd.GettingCategoryName:
                    if (await IsNotCorrectInput(userInput, botClient, chatId, cancellationToken, user))
                        break;

                    _newUserCategory[chatId].Name = userInput;
                    user.StateAdd = StateAdd.GettingCategorySubcategory;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringCategorySubcategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(null, user));
                    break;

                case StateAdd.GettingCategorySubcategory:
                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringCategorySubcategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(GetCategoryByName(message.Text), user));
                    break;

                case StateAdd.End:
                    _newUserCategory[chatId].Subcategory = GetCategoryByName(message.Text);

                    await CreateCategotyAsync(_newUserCategory[chatId]);

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
