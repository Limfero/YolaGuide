using Telegram.Bot.Types.ReplyMarkups;
using YolaGuide.Controllers;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;

namespace YolaGuide.Messages
{
    public static class Keyboard
    {
        public static InlineKeyboardMarkup LanguageChange { get; private set; } = new(new List<InlineKeyboardButton[]>()
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Русский"),
                InlineKeyboardButton.WithCallbackData("English")
            }
        });


        public static ReplyKeyboardMarkup GetStart()
        {
            return new(new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "План на сегодня", "Today's plan" }[(int)Settings.Language]),
                    new KeyboardButton(new List<string>(){ "Найти место по названию", "Find a place by name" }[(int)Settings.Language]),
                    new KeyboardButton(new List<string>(){ "Насторойки", "Settings" }[(int)Settings.Language])
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Дай факт!", "Give me a fact!" }[(int)Settings.Language]),
                    new KeyboardButton(new List<string>(){ "Порекомендуй место!", "Recommend a place!" }[(int)Settings.Language])
                }
            })
            { ResizeKeyboard = true };
        }

        public static InlineKeyboardMarkup SelectAdministrator() 
        { 
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Админ панель", "Admin panel" }[(int)Settings.Language], "Админ панель"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Главное меню", "Main menu" }[(int)Settings.Language], "Главное меню")
                }
            });
        }

        public static InlineKeyboardMarkup ChoosingWhatToAdd()
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Место", "Place" }[(int)Settings.Language], "Место"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Категорию", "Category" }[(int)Settings.Language], "Категорию"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршрут", "Route" }[(int)Settings.Language], "Маршрут"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Факт", "Fact" }[(int)Settings.Language], "Факт"),
                }
            });
        }

        public static InlineKeyboardMarkup CategorySelection(Category category)
        {
            var categories = CategoryController.GetCategories(category).ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var c in categories)
            {
                var listNamesInDiffLanguages = c.Name.Split("\n\n");

                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton> 
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)Settings.Language], c.Name) 
                }));
            }

            if(category == null || category.Subcategories.Count == 0)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)Settings.Language], "Я все выбрал!"),
                }));

            return keyboard;
        }

    }
}

