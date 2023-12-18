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

        public static List<string> ObjectSelection { get; set; } = new()
        {
            "Над каким объектом будем проводить операцию?",
            "What facility will we be operating on?"
        };

        public static List<string> ClarificationOfPreferences { get; set; } = new()
        {
            "Выберете категории, которые вам интересны:",
            "Choose the categories you are interested in:"
        };

        public static List<string> Added { get; set; } = new()
        {
            "Добавлено!",
            "Added!"
        };

        public static List<string> SuccessfullyAdded { get; set; } = new()
        {
            "Поздравляю! Ты смог добавить\nヽ(°□° )ノ\nКуда направимся теперь?",
            "Congratulations!!! You were able to add\nヽ(°□° )ノ\nWhere do we go now?"
        };

        public static List<string> SuccessfullyDellete { get; set; } = new()
        {
            "Успешно удалено!\nКуда направимся теперь?",
            "Successfully deleted!\nWhere do we go now?"
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
            "Введи адрес места(сначала на русском, потом через строчку на английском):",
            "Enter the address of the place(first in Russian, then a line later in English):"
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

        //Добавление маршрута
        public static List<string> EnteringRouteName { get; set; } = new()
        {
            "Введи название маршрута(сначала на русском, потом через строчку на английском):",
            "Enter the name of the route (first in Russian, then across the line in English):"
        };

        public static List<string> EnteringRouteDescription { get; set; } = new()
        {
            "Введи описание маршрута(сначала на русском, потом через строчку на английском):",
            "Enter a description of the route (first in Russian, then through a line in English):"
        };

        public static List<string> EnteringRouteCost { get; set; } = new()
        {
            "Введи стоимость маршрута, если он бесплатный, то укажи 0:",
            "Enter the cost of the route, if it is free, then specify 0:"
        };

        public static List<string> EnteringRouteTelephone { get; set; } = new()
        {
            "Введи номер телефона, где можно забронировать, если телефона нет, то напиши \"Нет\":",
            "Enter the phone number where you can make a reservation, and if you don't have a phone number, write \"No\":"
        };

        public static List<string> EnteringRoutePlaces { get; set; } = new()
        {
            "Осталось выбрать места, которые есть в маршруте:",
            "All that's left is to pick the places that are on the route:"
        };

        //Добавление в план
        public static List<string> MakingPlan { get; set; } = new()
        {
            "Выбирайте куда хотите отправиться сегодня:",
            "Choose where you want to go today:"
        };

        public static List<string> EnteringPlanPlace { get; set; } = new()
        {
            "А теперь выберете место:",
            "Now pick a place:"
        };

        public static List<string> EnteringPlanAdress { get; set; } = new()
        {
            "И осталось выбрать адрес места:",
            "And the only thing left to do is pick an address for the place:"
        };

        // Удаление
        public static List<string> DeletePlace { get; set; } = new()
        {
            "Выберете место, которое хотите удалить:",
            "Select the place you want to delete:"
        };

        public static List<string> DeleteCategory { get; set; } = new()
        {
            "Выберете категорию, которую хотите удалить:",
            "Select the category you want to delete:"
        };


        public static List<string> DeleteFact { get; set; } = new()
        {
            "Выберете факт, который хотите удалить:",
            "Select the fact you want to delete:"
        };

        public static List<string> DeleteRoute { get; set; } = new()
        {
            "Выберете маршрут, который хотите удалить:",
            "Select the route you want to delete:"
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

        public static string GetRouteInformation(Route route, Language language)
        {
            string[] name = route.Name.Split("\n\n");
            string[] description = route.Description.Split("\n\n");
            string cost = route.Cost == 0 ? "Бесплатно" : route.Cost.ToString();
            string telephone = route.Telephone;

            string[] messageCost = new[] { "Цена", "Cost" };
            string[] messageTelephone = new[] { "Телефон", "Telephone" };

            return string.Format("{0}\n{1}\n{2} {3}\n{4} {5}", name[(int)language], description[(int)language], messageCost[(int)language], cost[(int)language], messageTelephone[(int)language], telephone[(int)language]);
        }

        public static string GetPlanInformation(User user)
        {
            var placesInPlan = user.Places;
            string result = PlacesInPlan[(int)user.Language];

            for(int i = 0; i < placesInPlan.Count; i++) 
            {
                var name = placesInPlan[i].Name.Split("\n\n")[(int)user.Language];
                var adress = placesInPlan[i].Adress.Split("\n\n")[(int)user.Language];

                result += $"{i + 1}) {name}\n{adress}\n\n";
            }

            return result;
        }
    }
}
