namespace YolaGuide.Messages
{
    public static class Answer
    {
        public static List<string> WelcomeMessage { get; set; } = new()
        { 
            "Привет, я Yola Guide! Я расскажу тебе о всех местах Йошкар-Олы.",
            "Hi, I'm Yola Guide! I will tell you about all the places in Yoshkar-Ola."
        };

        public static List<string> SelectAdmin { get; set; } = new()
        {
            "Куда идем, магистр?",
            "Where are we going, Master?"
        };

        public static List<string> WhatToAdd { get; set; } = new()
        {
            "Что будем добавлять, сер?",
            "What do we add, sir?"
        };

        public static List<string> Continue { get; set; } = new()
        {
            "~продолжаем~",
            "~continue~"
        };

        public static List<string> ErrorInput { get; set; } = new()
        {
            "Ой... Ошибочка в формате написания. Пробуй ещё раз, пожалуйста!",
            "Oops... Wrong spelling format. Try again, please!"
        };

        // Добавление места
        public static List<string> EnteringPlaceName { get; set; } = new()
        {
            "Введи имя нового места(сначала русский потом через пробел английский):",
            "Enter the name of the new place(first Russian then English with a space):"
        };

        public static List<string> EnteringPlaceDescription { get; set; } = new()
        {
            "Введи описание нового места(сначала на русском, потом через строчку на английском):",
            "Enter the description of the new place(first in Russian, then a line later in English):"
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
            "Введи координаты в формате - долгота,широта :",
            "Enter coordinates in the format - longitude,latitude :"
        };

        public static List<string> EnteringPlaceCategories{ get; set; } = new()
        {
            "Осталось только выбрать категории",
            "All that's left is to choose the filters:"
        };

        public static List<string> PlaceSuccessfullyAdded { get; set; } = new()
        {
            "Поздравляю! Ты смог добавить целое место ヽ(°□° )ノ\n\nКуда направимся теперь?",
            "Congratulations!!! You were able to add a whole ヽ(°□° )ノ\n\nWhere do we go now?"
        };
    }
}
