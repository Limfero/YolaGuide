using System;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;

namespace YolaGuide.Messages
{
    public static class Answer
    {
        public static List<string> WelcomeMessage { get; set; } = new()
        { 
            "Привет, я Yola Guide! Я расскажу тебе о всех местах Йошкар-Олы.",
            "Hi, I'm Yola Guide! I will tell you about all the places in Yoshkar-Ola."
        };

        public static List<string> SelectingMenuButton { get; set; } = new()
        {
            "Можете выбрать куда отправиться в меню!",
            "You can pick where to go on the menu!"
        };

        public static List<string> Settings { get; set; } = new()
        {
            "Что желаете настроить?",
            "What do you wish to customize?"
        };

        public static List<string> SuccessfullySetUpPreferences { get; set; } = new()
        {
            "Предпочтения успешно настроены!",
            "Preferences have been successfully set up!"
        };

        public static List<string> SelectAdmin { get; set; } = new()
        {
            "Куда идем, магистр?",
            "Where are we going, Master?"
        };

        public static List<string> WhatToAdd { get; set; } = new()
        {
            "Что будем добавлять?",
            "What do we add?"
        };

        public static List<string> ClarificationOfPreferences { get; set; } = new()
        {
            "Выберете категории, которые вам интересны:",
            "Choose the categories you are interested in:"
        };

        public static List<string> SuccessfullyAdded { get; set; } = new()
        {
            "Поздравляю! Ты смог добавить\nヽ(°□° )ノ\nКуда направимся теперь?",
            "Congratulations!!! You were able to add\nヽ(°□° )ノ\nWhere do we go now?"
        };

        public static List<string> NothingToOffer { get; set; } = new()
        {
            "Нам нечего вам предложить :с",
            "We have nothing to offer you :c"
        };

        public static List<string> Loading { get; set; } = new()
        {
            "Загрузка....",
            "Loading...."
        };

        public static List<string> PlacesInPlan { get; set; } = new()
        {
            "В вашем плане на сегодня:\n",
            "In your plan for today:\n"
        };

        // Ошибочки
        public static List<string> WrongCommand { get; set; } = new()
        {
            "К сожалению, я ещё тупенький и не знаю этой команды",
            "Unfortunately, I'm still dumb and don't know this command yet"
        };

        public static List<string> WrongInputType { get; set; } = new()
        {
            "Красивое... Но, к сожалению, я понимаю только текст :с",
            "It's beautiful... But unfortunately, I can only understand the text :с"
        };

        public static List<string> WrongLanguage { get; set; } = new()
        {
            "Я не знаю такого языка... :с",
            "I don't know that kind of language... :с"
        };

        public static List<string> WrongInputFormat { get; set; } = new()
        {
            "Ой... Ошибочка в формате написания. Пробуй ещё раз, пожалуйста!",
            "Oops... Wrong spelling format. Try again, please!"
        };

        public static List<string> Error { get; set; } = new()
        {
            "Упс... Я такого не знаю. Попробуйте выбрать из меню выше!",
            "Oops... I don't know that one. Try selecting from the menu above!"
        };

        // Добавление места
        public static List<string> EnteringPlaceName { get; set; } = new()
        {
            "Введи имя нового места(сначала на русском, потом через строчку на английском):",
            "Enter the name of the new place(first Russian then English with a space):"
        };

        public static List<string> EnteringPlaceDescription { get; set; } = new()
        {
            "Введи описание места(сначала на русском, потом через строчку на английском):",
            "Enter the description of the place(first in Russian, then a line later in English):"
        };

        public static List<string> EnteringPlaceAdress { get; set; } = new()
        {
            "Введи адрес места:",
            "Enter the address of the place:"
        };


        public static List<string> EnteringPlaceContactInformation { get; set; } = new()
        {
            "Введи контактную информацию для этого места(сначала на русском, потом через строчку на английском):",
            "Enter the contact information for this place (first in Russian, then across the line in English):"
        };

        public static List<string> EnteringPlaceImage { get; set; } = new()
        {
            "Отправь фотографию на обложку места:",
            "Send a picture of yourself on the cover of the place:"
        };

        public static List<string> EnteringPlaceYId { get; set; } = new()
        {
            "Введи id организации в Яндекс Картах:",
            "Enter the id of the organization in Yandex Maps:"
        };

        public static List<string> EnteringPlaceCoordinates { get; set; } = new()
        {
            "Введи координаты:",
            "Enter coordinates:"
        };

        public static List<string> EnteringPlaceCategories{ get; set; } = new()
        {
            "Осталось только выбрать категории",
            "All that's left is to choose the filters:"
        };

        // Добавление категории
        public static List<string> EnteringCategoryName { get; set; } = new()
        {
            "Введи название новой категории(сначала на русском, потом через строчку на английском):",
            "Enter the name of the new category(first Russian then English with a space):"
        };

        public static List<string> EnteringCategorySubcategory { get; set; } = new()
        {
            "Осталось понять, будет ли эта категория подкатегорией? Если да, то выбери какой!",
            "It remains to be seen if this category will be a subcategory? If so, pick which one!"
        };

        //Добавление фатка
        public static List<string> EnteringFactName { get; set; } = new()
        {
            "Придумай название фактика(сначала на русском, потом через строчку на английском):",
            "Think up a name for the factoid(first in Russian, then across the line in English):"
        };

        public static List<string> EnteringFactDescription { get; set; } = new()
        {
            "Ну и давай свой факт(сначала на русском, потом через строчку на английском):",
            "So give me your fact(first in Russian, then across the line in English):"
        };


        //Добавление в план
        public static List<string> NoPlanForToday { get; set; } = new()
        {
            "У вас пока нет планов. Пора составить!\n\nВыбирайте куда хотите отправиться сегодня:",
            "You don't have plans yet. It's time to make some!\n\nChoose where you want to go today:"
        };

        public static List<string> EnteringPlanCategory { get; set; } = new()
        {
            "А теперь выберете место:",
            "Now pick a place:"
        };

        public static List<string> EnteringPlanAdress { get; set; } = new()
        {
            "И остаось выбрать адрес места:",
            "And the only thing left to do is pick an address for the place:"
        };

        //Поиск
        public static List<string> EnteringStringToSearch { get; set; } = new()
        {
            "Введите то, что хотите найти:",
            "Enter what you want to find:"
        };

        public static string GetPlaceInformation(Place place, Language language)
        {
            string[] name = place.Name.Split("\n\n");
            string[] description = place.Description.Split("\n\n");

            return string.Format("{0}\n{1}", name[(int)language], description[(int)language]);
        }

        public static string GetFactInformation(Fact fact, Language language)
        {
            string[] name = fact.Name.Split("\n\n");
            string[] description = fact.Description.Split("\n\n");

            return string.Format("{0}\n{1}", name[(int)language], description[(int)language]);
        }

        public static string GetPlanInformation(User user)
        {
            var placesInPlan = user.Places;
            string result = PlacesInPlan[(int)user.Language];

            for(int i = 0; i < placesInPlan.Count; i++) 
            {
                var name = placesInPlan[i].Name.Split("\n\n")[(int)user.Language];
                var adress = placesInPlan[i].Adress;

                result += $"{i + 1}) {name}\n{adress}\n\n";
            }

            return result;
        }
    }
}
