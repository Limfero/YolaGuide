namespace YolaGuide.Response
{
    public static class Message
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

        public static List<string> EnteringPlaceName { get; set; } = new()
        {
            "Введи имя нового места:",
            "Enter the name of the new place:"
        };

        public static List<string> EnteringPlaceDescription { get; set; } = new()
        {
            "Введи описание нового места:",
            "Enter the description of the new place:"
        };
    }
}
