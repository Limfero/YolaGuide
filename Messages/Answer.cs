namespace YolaGuide.Messages
{
    public static class Answer
    {
        public static List<string> WelcomeMessage { get; set; } = new()
        { 
            "Привет, я Yola Guide! Я расскажу тебе о всех местах Йошкар-Олы.",
            "Hi, I'm Yola Guide! I will tell you about all the places in Yoshkar-Ola."
        };

        public static List<string> WrongCommand { get; set; } = new()
        {
            "К сожалению, я ещё тупенький и не знаю этой команды",
            "Unfortunately, I'm still dumb and don't know this command yet"
        };

        public static List<string> WrongInputFormat { get; set; } = new()
        {
            "Красивое... Но, к сожалению, я понимаю только текст :с",
            "It's beautiful... But unfortunately, I can only understand the text :s"
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

        public static List<string> ErrorInput { get; set; } = new()
        {
            "Ой... Ошибочка в формате написания. Пробуй ещё раз, пожалуйста!",
            "Oops... Wrong spelling format. Try again, please!"
        };

        public static List<string> SuccessfullyAdded { get; set; } = new()
        {
            "Поздравляю! Ты смог добавить\nヽ(°□° )ノ\nКуда направимся теперь?",
            "Congratulations!!! You were able to add\nヽ(°□° )ノ\nWhere do we go now?"
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
            "Введи адрес этого места:",
            "Enter the address of this place:"
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
    }
}
