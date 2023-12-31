﻿using Telegram.Bot.Types.ReplyMarkups;
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
                InlineKeyboardButton.WithCallbackData("Русский 🇷🇺", "Русский"),
                InlineKeyboardButton.WithCallbackData("English 🇺🇸", "English")
            }
        });

        public static InlineKeyboardMarkup Back(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static InlineKeyboardMarkup SuccessfullyAdded(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Добавить ещё 🆕", "Add more 🆕" }[(int)language], "Назад"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static InlineKeyboardMarkup Back(Language language, string callback)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад\n{callback}"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static InlineKeyboardMarkup Fact(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Ещё факт! 🤔", "Another fact! 🤔" }[(int)language], "Дай факт!"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static ReplyKeyboardMarkup ReplyMenu(long chatId, Language language)
        {
            var keyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "О городе 🏙️", "About the city 🏙️" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Готовые маршруты 🛣️", "Prepared routes 🛣️" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Афиша 📃", "Playbill 📃" }[(int)language]),
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Запланировать поездку 👜", "Plan a trip 👜" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Найти места 🔍", "Find places 🔍" }[(int)language])
                },

                new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Настройки ⚙️", "Settings ⚙️" }[(int)language]),
                    new KeyboardButton(new List<string>(){ "Для маломобильного гостя 🦿", "For a guest with low mobility 🦿" }[(int)language]),
                }
            })
            { ResizeKeyboard = true };

            if (Settings.Admins.Contains(chatId))
                keyboard = new ReplyKeyboardMarkup(keyboard.Keyboard.Append(new KeyboardButton[]
                {
                    new KeyboardButton(new List<string>(){ "Админ панель 😎", "Admin panel 😎" }[(int)language]),
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup InlineMenu(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "План на сегодня 📋", "Today's plan 📋" }[(int)language], "План на сегодня"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Поиск 🔍", "Search 🔍" }[(int)language], "Поиск")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Дай факт! 🤔", "Give me a fact! 🤔" }[(int)language], "Дай факт!"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Посоветуй место! 🙏", "Recommend a place! 🙏" }[(int)language], "Посоветуй место!")
                },
            });
        }

        public static InlineKeyboardMarkup AboutCity(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Как добраться? 👣", "How to get there? 👣" }[(int)language], "Как добраться?"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Что посмотреть? Достопримечательности 👀", "What to see? Attractions 👀" }[(int)language], "Что посмотреть?")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Что попробовать? 🍽️", "What to try from food? 🍽️" }[(int)language], "Что попробовать?")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Где жить? 🏡", "Where to live? 🏡" }[(int)language], "Где жить?")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Что привезти? 🎁", "What can I bring? 🎁" }[(int)language], "Что привезти?")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static InlineKeyboardMarkup ClarificationPreferences(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Да, хочу! ✅", "Yes, I do! ✅" }[(int)language], "Уточнение предпочтений"),
                },
                
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Нет, мне нравиться все! 😎", "No, I like everything! 😎" }[(int)language], "Открыть меню")
                }
            });
        }

        public static InlineKeyboardMarkup GetSettings(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Предпочтения 🥰", "Preferences 🥰" }[(int)language], "Уточнение предпочтений"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ $"Язык {(language == Language.Russian ? "🇷🇺" : "🇺🇸")}", "Language" }[(int)language], "Уточнение языка")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                },
            });
        }

        public static InlineKeyboardMarkup SelectToDeleteOrAdd(Language language, string whatDeleteOrAdd)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Добавить 🆕", "Add 🆕" }[(int)language], $"Добавить\n{whatDeleteOrAdd}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Удалить 🗑️", "Delete 🗑️" }[(int)language], $"Удалить\n{whatDeleteOrAdd}")
                },

                new InlineKeyboardButton[]
                {
                     InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                }
            });
        }

        public static InlineKeyboardMarkup ChoosingWhatToAdd(Language language, string operation)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Место 🏞️", "Place 🏞️" }[(int)language], $"{operation} место"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Категорию 📚", "Category 📚" }[(int)language], $"{operation} категорию")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршрут 🛣️", "Route 🛣️" }[(int)language], $"{operation} маршрут"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Факт 🤔", "Fact 🤔" }[(int)language], $"{operation} факт")
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад в меню 🏠", "Back to menu 🏠" }[(int)language], "Открыть меню"),
                }
            });
        }

        public static InlineKeyboardMarkup SuccessfullyCustomizedPreferences(Language language)
        {
            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Заново настроить! ↩️", "Readjust! ↩️" }[(int)language], "Уточнение предпочтений"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню! 🏠", "On the menu! 🏠" }[(int)language], "Открыть меню"),
                },
            });
        }

        public static InlineKeyboardMarkup GetPlaceCard(Language language, Place place, Domain.Entity.User user, string message)
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Контактная информация ℹ️", "Contact information ℹ️" }[(int)language], $"Контактная информация\n{place.Id}"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Маршруты 🛣️", "Routes 🛣️" }[(int)language], $"Маршруты\n{place.Id}"),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl(new List<string>(){ "Показать на карте 🗺️", "Show on map 🗺️" }[(int)language], string.Format(Settings.URLYandexOrganization, place.YIdOrganization)),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Места рядом 🏘️", "Places nearby 🏘️" }[(int)language], $"Места рядом\n{place.Id}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Похожие места 🤲", "Related places 🤲" }[(int)language], $"Похожие места\n{place.Id}"),
                },
            });

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Посоветуй другое место! 🏞️", "Recommend another place! 🏞️" }[(int)language], "Посоветуй место!"),
            }));

            if (user.Places.FirstOrDefault(p => p.Id == place.Id) == null)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Добавить в план на сегодня ✅", "Add to today's plan ✅" }[(int)language], $"В план\n{place.Id}"),
                }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                 InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                 InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetPlanCard(Language language, Domain.Entity.User user)
        {
            var keyboard = GetRouteButton(user.Places, language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "Редактировать план ✏️", "Edit the plan ✏️" }[(int)language], "Редактировать план"),
            }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(new List<string>() { "Очистить план 🫥", "Contact information 🫥" }[(int)language], "Очистить план"),
            }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetRouteCard(Language language, Route route)
        {
            var keyboard = GetRouteButton(route.Places, language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup CategorySelection(Category category, Language language)
        {
            var keyboard = GetKeyboardSubcategories(category, language);

            if (category == null)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал! ✅", "I chose everything! ✅" }[(int)language], category == null ? $"Я все выбрал!\n" : $"Я все выбрал!\n{category.Id}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню")
                }));
            else
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал! ✅", "I chose everything! ✅" }[(int)language], category == null ? $"Я все выбрал!\n" : $"Я все выбрал!\n{category.Id}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>() { "Назад ↩️", "Back ↩️" }[(int)language], "Назад")
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup PlanCategorySelection(Category category, Language language)
        {
            var keyboard = GetKeyboardSubcategories(category, language);

            if(category != null)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В план 📋", "Into the plan 📋" }[(int)language], "В план на сегодня"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад")
                }));
            else
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В план 📋", "Into the plan 📋" }[(int)language], "В план на сегодня"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup PreferenceSelection(Category category, Language language, Domain.Entity.User user)
        {
            var subcategories = Settings.CategoryController.GetCategories(category).ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var subcategory in subcategories)
            {
                if (user.Categories.FirstOrDefault(category => category.Id == subcategory.Id) == null)
                {
                    var listNamesInDiffLanguages = subcategory.Name.Split("\n\n\n");

                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)language], subcategory.Id.ToString())
                    }));
                }
            }
            
            if(category == null)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал! ✅", "I chose everything! ✅" }[(int)language], category == null ? $"Я все выбрал!\n" : $"Я все выбрал!\n{category.Id}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню")
                }));
            else
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал! ✅", "I chose everything! ✅" }[(int)language], category == null ? $"Я все выбрал!\n" : $"Я все выбрал!\n{category.Id}"),
                    InlineKeyboardButton.WithCallbackData(new List<string>() { "Назад ↩️", "Back ↩️" }[(int)language], "Назад")
                }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetPlaceAddressesByName(string name, Language language)
        {
            var places = Settings.PlaceController.GetPlacesByName(name);
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var p in places)
            {
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(p.Adress.Split("\n\n\n")[(int)language], p.Id.ToString())
                }));
            }

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetPlaceInList(Language language, List<Place> places)
        {
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var place in places)
                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(place.Name.Split("\n\n\n")[(int)language], place.Id.ToString()),
                }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetNamePlaceByCategory(Category category, Language language, int numberPage)
        {
            var places = Settings.PlaceController.GetPlacesByCategory(category).Select(place => place.Name).Distinct().ToList();

            var keyboard = GetNavigationButton(places, numberPage, category.Name.Split("\n\n\n")[(int)language], language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetResultSearch(Language language, string userInput, int numberPage)
        {
            var places = Settings.PlaceController.SearchPlace(userInput).Select(place => place.Name).Distinct().ToList();

            var keyboard = GetNavigationButton(places, numberPage, userInput, language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetAllPlace(Language language, int numberPage)
        {
            var places = Settings.PlaceController.GetAll().Select(place => place.Name).Distinct().ToList();

            var keyboard = GetNavigationButton(places, numberPage, "all", language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetAllPlacesWithoutRoute(Language language, int numberPage, List<Place> placeInRoute)
        {
            var responce = Settings.PlaceController.GetAll().Distinct().ToList();
            var places = new List<Place>();

            foreach (var place in responce)
                if (!placeInRoute.Contains(place))
                    places.Add(place);

            int countPage = (int)Math.Ceiling((double)places.Count / Settings.NumberObjectsPerPage) == 0 ? 1 : (int)Math.Ceiling((double)places.Count / Settings.NumberObjectsPerPage);
            int startPosition = (numberPage - 1) * Settings.NumberObjectsPerPage;
            int endPosition = numberPage * Settings.NumberObjectsPerPage > places.Count ? places.Count : numberPage * Settings.NumberObjectsPerPage;
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            if (places.Count > 0)
                for (int i = startPosition; i < endPosition; i++)
                {
                    var name = places[i].Name.Split("\n\n\n")[(int)language];
                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(name, name[0..(name.Length >= Settings.MaxLengthInliteButton ? Settings.MaxLengthInliteButton : name.Length)])
                    }));
                }

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData("<", numberPage == 1 ? $"all 1" : $"all {numberPage - 1}"),
                   InlineKeyboardButton.WithCallbackData($"{numberPage}/{countPage}"),
                   InlineKeyboardButton.WithCallbackData(">", numberPage == countPage ? $"all {countPage}" : $"all {numberPage + 1}"),
            }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "Я все выбрал! ✅", "I chose everything! ✅" }[(int)language], $"Я все выбрал!"),
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], "Назад"),
                InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetAllFacts(Language language, int numberPage)
        {
            var facts = Settings.FactController.GetAll().Select(fact => fact.Name).Distinct().ToList();

            var keyboard = GetNavigationButton(facts, numberPage, "all", language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetAllRoutes(Language language, int numberPage)
        {
            var routes = Settings.RouteController.GetAll().Select(fact => fact.Name).ToList();

            var keyboard = GetNavigationButton(routes, numberPage, "all", language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GetRoutesByPlace(Language language, int numberPage, Place place, string callback)
        {
            var routes = Settings.RouteController.GetAll().Where(route => route.Places.FirstOrDefault(p => p.Id == place.Id) != null).Select(route => route.Name).ToList();

            var keyboard = GetNavigationButton(routes, numberPage, callback, language);

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад\n{place.Id}"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        public static InlineKeyboardMarkup GettingPlacesInListAndButtonNavigation(Language language, int numberPage, List<Place> places, string callback)
        {
            int countPage = (int)Math.Ceiling((double)places.Count / Settings.NumberObjectsPerPage) == 0 ? 1 : (int)Math.Ceiling((double)places.Count / Settings.NumberObjectsPerPage);
            int startPosition = (numberPage - 1) * Settings.NumberObjectsPerPage;
            int endPosition = numberPage * Settings.NumberObjectsPerPage > places.Count ? places.Count : numberPage * Settings.NumberObjectsPerPage;
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());
            string listPlaces = "";

            foreach (Place place in places)
                listPlaces += place.Id + " ";

            if (places.Count > 0)
                for (int i = startPosition; i < endPosition; i++)
                {
                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(places[i].Name.Split("\n\n\n")[(int)language], places[i].Id.ToString())
                    }));
                }

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData("<", numberPage == 1 ? $"{listPlaces[0..(listPlaces.Length - 1)]}\n1" : $"{listPlaces[0..(listPlaces.Length - 1)]}\n{numberPage - 1}"),
                   InlineKeyboardButton.WithCallbackData($"{numberPage}/{countPage}", $"Назад\n{callback}"),
                   InlineKeyboardButton.WithCallbackData(">", numberPage == countPage ? $"{listPlaces[0..(listPlaces.Length - 1)]}\n{countPage}" : $"{listPlaces[0..(listPlaces.Length - 1)]}\n{numberPage + 1}"),
            }));

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "Назад ↩️", "Back ↩️" }[(int)language], $"Назад\n{callback}"),
                   InlineKeyboardButton.WithCallbackData(new List<string>(){ "В меню 🏠", "On the menu 🏠" }[(int)language], "Открыть меню"),
            }));

            return keyboard;
        }

        private static InlineKeyboardMarkup GetKeyboardSubcategories(Category category, Language language)
        {
            var subcategories = Settings.CategoryController.GetCategories(category).ToList();
            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            foreach (var subcategory in subcategories)
            {
                var listNamesInDiffLanguages = subcategory.Name.Split("\n\n\n");

                keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>() { listNamesInDiffLanguages[(int)Language.Russian], listNamesInDiffLanguages[(int)Language.English] }[(int)language], subcategory.Id.ToString())
                }));
            }

            return keyboard;
        }

        private static InlineKeyboardMarkup GetNavigationButton(List<string> names, int numberPage, string callBack, Language language)
        {
            int countPage = (int)Math.Ceiling((double)names.Count / Settings.NumberObjectsPerPage) == 0 ? 1 : (int)Math.Ceiling((double)names.Count / Settings.NumberObjectsPerPage);
            int startPosition = (numberPage - 1) * Settings.NumberObjectsPerPage;
            int endPosition = numberPage * Settings.NumberObjectsPerPage > names.Count ? names.Count : numberPage * Settings.NumberObjectsPerPage;

            var keyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>());

            if(names.Count  > 0) 
                for (int i = startPosition; i < endPosition; i++)
                {
                    var name = names[i].Split("\n\n\n")[(int)language];
                    keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(name, name[0..(name.Length >= Settings.MaxLengthInliteButton ? Settings.MaxLengthInliteButton : name.Length)])
                    }));
                }

            keyboard = new InlineKeyboardMarkup(keyboard.InlineKeyboard.Append(new List<InlineKeyboardButton>
            {
                   InlineKeyboardButton.WithCallbackData("<", numberPage == 1 ? $"{callBack} 1" : $"{callBack} {numberPage - 1}"),
                   InlineKeyboardButton.WithCallbackData($"{numberPage}/{countPage}", "Назад"),
                   InlineKeyboardButton.WithCallbackData(">", numberPage == countPage ? $"{callBack} {countPage}" : $"{callBack} {numberPage + 1}"),
            }));

            return keyboard;
        }

        private static InlineKeyboardMarkup GetRouteButton(List<Place> placesInRoute, Language language)
        {
            var coordinats = "";
            foreach (var place in placesInRoute)
                coordinats += place.Coordinates + "~";

            var places = "";
            foreach (var place in placesInRoute)
                places += place.Id + " ";

            return new(new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(new List<string>(){ "Показать места 🏘️", "Show the place 🏘️" }[(int)language],  places[0..(places.Length - 1)]),
                },

                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl(new List<string>(){ "Показать на карте 🗺️", "Show on map 🗺️" }[(int)language], string.Format(Settings.URLYandexRoute, coordinats[0..(coordinats.Length - 1)])),
                },
            });
        }
    }
}

