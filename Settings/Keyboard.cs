using Telegram.Bot.Types.ReplyMarkups;
using YolaGuide.Domain.Enums;

namespace YolaGuide.Settings
{
    public static class Keyboard
    {
        public static InlineKeyboardMarkup LanguageChangeKeyboard { get; private set; } = new(new List<InlineKeyboardButton[]>()
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Русский"),
                InlineKeyboardButton.WithCallbackData("English")
            }
        });


        public static ReplyKeyboardMarkup GetStartKeyboard(Language language)
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
    }
}

