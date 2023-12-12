using Telegram.Bot.Types;
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


        public static ReplyKeyboardMarkup GetStart(long chatId, Domain.Entity.User user)
        {
            var keyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "План на сегодня", "Today's plan" }[(int)user.Language]),
                    new KeyboardButton(new List<string>(){ "Все места", "All place" }[(int)user.Language]),
                    new KeyboardButton(new List<string>(){ "Поиск", "Search" }[(int)user.Language]),
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Дай факт!", "Give me a fact!" }[(int)user.Language]),
                    new KeyboardButton(new List<string>(){ "Посоветуй место!", "Recommend a place!" }[(int)user.Language])
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Настройки", "Settings" }[(int)user.Language]),
                    new KeyboardButton(new List<string>(){ "О городе", "About  the city" }[(int)user.Language])
                }
            })
            { ResizeKeyboard = true };

            if (Settings.Admins.Contains(chatId))
                keyboard = new ReplyKeyboardMarkup(keyboard.Keyboard.Append(new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Админ панель", "Admin panel" }[(int)user.Language]),
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup ClarificationPreferences(Domain.Entity.User user)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Да, хочу!", "Yes, I do!" }[(int)user.Language], "Уточнение предпочтений"),
                },
                
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Нет, мне нравиться все!", "No, I like everything!" }[(int)user.Language], "Главное меню")
                }
            });
        }

        public static InlineKeyboardMarkup GetSettings(Domain.Entity.User user)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Предпочтения", "Preferences" }[(int)user.Language], "Уточнение предпочтений"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Язык", "Language" }[(int)user.Language], "Уточнение языка")
                },
            });
        }

        public static InlineKeyboardMarkup ChoosingWhatToAdd(Domain.Entity.User user)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Место", "Place" }[(int)user.Language], "Место"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Категорию", "Category" }[(int)user.Language], "Категорию"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршрут", "Route" }[(int)user.Language], "Маршрут"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Факт", "Fact" }[(int)user.Language], "Факт"),
                }
            });
        }

        public static InlineKeyboardMarkup CategorySelection(Category category, Domain.Entity.User user)
        {
            var categories = CategoryController.GetCategories(category).ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var c in categories)
            {
                var listNamesInDiffLanguages = c.Name.Split("\n\n");

                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton> 
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)user.Language], c.Name) 
                }));
            }

            if(category == null || category.Subcategories.Count == 0)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)user.Language], "Я все выбрал!"),
                }));

            return keyboard;
        }

    }
}

