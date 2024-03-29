﻿using System.Reflection;
using Telegram.Bot.Types;
using YolaGuide.Controllers;

namespace YolaGuide
{
    public static class Settings
    {
        public static string ConnectionString { get { return "Your Connection String"; } }

        public static string Token { get { return "Your Token"; } }

        public static string CurrentDirectory { get { return Assembly.GetExecutingAssembly().Location; } }

        public static string DestinationImagePath { get { return CurrentDirectory[0..CurrentDirectory.IndexOf("YolaGuide")] + "YolaGuide\\Image\\"; } }

        public static string URLYandexOrganization { get { return "https://yandex.ru/maps/org/{0}"; } }

        public static string URLYandexRoute { get { return "https://yandex.ru/maps/?rtext={0}&rtt=mt"; } }

        public static int NumberObjectsPerPage { get { return 3; } }

        public static int MaxLengthInliteButton { get { return 32; } }

        public static int DistanceBetweenTwoLocations { get { return 200; } }

        public static List<long> Admins { get { return new() { }; } }

        public static Dictionary<long, Message> LastBotMsg = new();

        public static Message DeleteMessage = null;

        public static UserController UserController;

        public static FactController FactController;

        public static PlaceController PlaceController;

        public static RouteController RouteController;

        public static CategoryController CategoryController;
    }
}
