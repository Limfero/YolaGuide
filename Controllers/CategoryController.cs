using Telegram.Bot.Types;
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

        public static async Task CreateAsync(CategoryViewModel model)
        {
            var response = await _categoryService.CreateCategoryAsync(model);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task RemoveAsync(Category category)
        {
            var response = await _categoryService.RemoveCategoryAsync(category);

            if (response.StatusCode != StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task AddCategoryAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.StartAddCategory:
                    user.Substate = Substate.GettingCategoryName;

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringCategoryName[(int)user.Language],
                        cancellationToken: cancellationToken);
                    break;
                case Substate.GettingCategoryName:
                    if (await BaseController.IsNotCorrectInput(userInput, botClient, cancellationToken, user))
                        break;

                    _newUserCategory[chatId].Name = userInput;
                    user.Substate = Substate.GettingCategorySubcategory;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.EnteringCategorySubcategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(null, user.Language));
                    break;

                case Substate.GettingCategorySubcategory:
                    if (GetCategoryByName(message.Text) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.EnteringCategorySubcategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(GetCategoryByName(message.Text), user.Language));
                    break;

                case Substate.End:
                    _newUserCategory[chatId].Subcategory = GetCategoryByName(message.Text);

                    await CreateAsync(_newUserCategory[chatId]);

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

        public static async Task ClarificationOfPreferencesAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.StartRefiningPreferences:
                    user.Substate = Substate.GettingPreferencesCategories;

                    if (user == null)
                    {
                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                            messageId: Settings.LastBotMsg[chatId].MessageId,
                            chatId: chatId,
                            text: Answer.WelcomeMessage[(int)user.Language],
                            cancellationToken: cancellationToken,
                            replyMarkup: Keyboard.ClarificationPreferences(user.Language));
                        break;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: Answer.ClarificationOfPreferences[(int)user.Language],
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.PreferenceSelection(null, user.Language));
                    break;
                case Substate.GettingPreferencesCategories:
                    var category = GetCategoryByName(message.Text);

                    if (GetCategoryByName(message.Text) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    if (category.Subcategories.Count == 0)
                    {
                        user.Categories.Add(category);
                        category = null;
                    }

                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: Answer.ClarificationOfPreferences[(int)user.Language],
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.PreferenceSelection(category, user.Language));
                    break;
                case Substate.End:
                    Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                           messageId: Settings.LastBotMsg[chatId].MessageId,
                           chatId: chatId,
                           text: Answer.SuccessfullySetUpPreferences[(int)user.Language],
                           cancellationToken: cancellationToken,
                           replyMarkup: Keyboard.SuccessfullyCustomizedPreferences(user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }

        public static async Task DeleteCategoryAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, Domain.Entity.User user)
        {
            var userInput = message.Text;
            var chatId = message.Chat.Id;

            switch (user.Substate)
            {
                case Substate.StartDeleteCategory:
                    user.Substate = Substate.GettingCategoryToDelete;

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.DeleteCategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(null, user.Language));
                    break;

                case Substate.GettingCategoryToDelete:
                    var category = GetCategoryByName(message.Text);

                    if (GetCategoryByName(message.Text) == null)
                    {
                        await BaseController.ShowError(botClient, cancellationToken, user);

                        break;
                    }

                    if (category.Subcategories.Count == 0)
                    {
                        await RemoveAsync(category);

                        user.State = State.Start;
                        user.Substate = Substate.Start;

                        Settings.LastBotMsg[chatId] = await botClient.EditMessageTextAsync(
                        messageId: Settings.LastBotMsg[chatId].MessageId,
                        chatId: chatId,
                        text: Answer.SuccessfullyDellete[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.Back(user.Language));
                    }

                    Settings.LastBotMsg[chatId] = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: Answer.DeleteCategory[(int)user.Language],
                        cancellationToken: cancellationToken,
                        replyMarkup: Keyboard.CategorySelection(category, user.Language));
                    break;
            }

            await UserController.UpdateUser(user);
        }
    }
}
