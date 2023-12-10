using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Service;
using static Azure.Core.HttpHeader;

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


        public static ReplyKeyboardMarkup GetStart(Language language)
        {
            return new(new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Категории", "Categories" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Закладки", "Bookmarks" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Поиск", "Search" }[(int)language])
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Дай факт!", "Give me a fact!" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Порекомендуй место!", "Recommend a place!" }[(int)language])
                }
            })
            { ResizeKeyboard = true };
        }

        public static InlineKeyboardMarkup SelectAdministrator(Language language) 
        { 
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Админ панель", "Admin panel" }[(int)language], "Админ панель"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Главное меню", "Main menu" }[(int)language], "Главное меню")
                }
            });
        }

        public static InlineKeyboardMarkup ChoosingWhatToAdd(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Место", "Place" }[(int)language], "Место"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Категорию", "Category" }[(int)language], "Категорию"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршрут", "Route" }[(int)language], "Маршрут"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Факт", "Fact" }[(int)language], "Факт"),
                }
            });
        }

        public static InlineKeyboardMarkup CategorySelection(Language language, Category category)
        {
            var categories = CategoryService.GetCategores(category).Data.ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var c in categories)
            {
                var listNamesInDiffLanguages = c.Name.Split(" ");

                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton> 
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)language], c.Name) 
                }));
            }

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)language], "Я все выбрал!"),
            }));

            return keyboard;
        }

    }
}

