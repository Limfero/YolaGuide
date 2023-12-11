﻿using System.Reflection;
using YolaGuide.Domain.Enums;

namespace YolaGuide
{
    public static class Settings
    {
        public static string ConnectionString { get { return "Data Source=DESKTOP-B3FAFAI\\SQLEXPRESS01;Initial Catalog=YolaGuide;TrustServerCertificate=True;Integrated Security=SSPI"; } }

        public static string Token { get { return "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4"; } }

        public static string CurrentDirectory { get { return Assembly.GetExecutingAssembly().Location; } }

        public static string DestinationImagePath { get { return CurrentDirectory[0..CurrentDirectory.IndexOf("YolaGuide")] + "YolaGuide\\Image\\"; } }

        public static List<long> Admins { get { return new() { 1059169240 }; } }

        public static Language Language { get; set; }
    }
}
