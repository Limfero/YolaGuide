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

        public static InlineKeyboardMarkup BackToMenu(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                 new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню", "Preferences" }[(int)language], "Главное меню"),
                }
            });
        }


        public static ReplyKeyboardMarkup GetStart(long chatId, Language language)
        {
            var keyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "План на сегодня", "Today's plan" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Все места", "All place" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Поиск", "Search" }[(int)language]),
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Дай факт!", "Give me a fact!" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Посоветуй место!", "Recommend a place!" }[(int)language])
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Настройки", "Settings" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "О городе", "About  the city" }[(int)language])
                }
            })
            { ResizeKeyboard = true };

            if (Settings.Admins.Contains(chatId))
                keyboard = new ReplyKeyboardMarkup(keyboard.Keyboard.Append(new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Админ панель", "Admin panel" }[(int)language]),
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup ClarificationPreferences(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Да, хочу!", "Yes, I do!" }[(int)language], "Уточнение предпочтений"),
                },
                
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Нет, мне нравиться все!", "No, I like everything!" }[(int)language], "Главное меню")
                }
            });
        }

        public static InlineKeyboardMarkup GetSettings(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Предпочтения", "Preferences" }[(int)language], "Уточнение предпочтений"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Язык", "Language" }[(int)language], "Уточнение языка")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню", "Back to menu" }[(int)language], "Главное меню"),
                },
            });
        }

        public static InlineKeyboardMarkup ChoosingWhatToAdd(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Место", "Place" }[(int)language], "Место"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Категорию", "Category" }[(int)language], "Категорию")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршрут", "Route" }[(int)language], "Маршрут"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Факт", "Fact" }[(int)language], "Факт")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню", "Back to menu" }[(int)language], "Главное меню"),
                }
            });
        }

        public static InlineKeyboardMarkup SuccessfullyCustomizedPreferences(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню!", "On the menu!" }[(int)language], "Главное меню"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Заново настроить!", "Readjust!" }[(int)language], "Уточнение предпочтений"),
                }
            });
        }

        public static InlineKeyboardMarkup GetPlaceCard(Language language, Place place)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Добавить в план на сегодня", "Add to today's plan" }[(int)language], "Добавить в план"),
                },

                new InlineKeyboardButton[] 
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Контактная информация", "Contact information" }[(int)language], "Контактная информарция"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршруты", "Маршруты" }[(int)language], "Маршруты"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl(new List<string>(){ "Показать на карте", "Show on map" }[(int)language], string.Format(Settings.URLYandexOrganization, place.YIdOrganization)),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Места рядом", "Places nearby" }[(int)language], "Места рядом"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Похожие места", "Related places" }[(int)language], "Похожие места"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню", "On the menu" }[(int)language], "Главное меню"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад", "Back" }[(int)language], "Назад"),
                }
            });
        }

        public static InlineKeyboardMarkup CategorySelection(Category category, Language language)
        {
            var keyboard = GetKeyboardSubcategories(category, language);

            if (category == null || category.Subcategories.Count == 0)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)language], "Я все выбрал!"),
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup PreferenceSelection(Category category, Language language)
        {
            var keyboard = GetKeyboardSubcategories(category, language);

            if (category == null)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал!", "I chose everything!" }[(int)language], "Я все выбрал!"),
                }));

            return keyboard;
        }

        private static InlineKeyboardMarkup GetKeyboardSubcategories(Category category, Language language)
        {
            var subcategories = CategoryController.GetCategories(category).ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var subcategory in subcategories)
            {
                var listNamesInDiffLanguages = subcategory.Name.Split("\n\n");

                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)language], subcategory.Name)
                }));
            }

            return keyboard;
        }

    }
}

