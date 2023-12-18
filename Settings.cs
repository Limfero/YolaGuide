using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Telegram.Bot.Types;
using YolaGuide.Domain.Enums;

namespace YolaGuide
{
    public static class Settings
    {
        public static string ConnectionString { get { return "Data Source=DESKTOP-B3FAFAI\\SQLEXPRESS01;Initial Catalog=YolaGuide;TrustServerCertificate=True;Integrated Security=SSPI"; } }

        public static string Token { get { return "6749828476:AAFa3mJUnpiX_9yC2-SUReuxzoSVIqM6Rh4"; } }

        public static string CurrentDirectory { get { return Assembly.GetExecutingAssembly().Location; } }

        public static string DestinationImagePath { get { return CurrentDirectory[0..CurrentDirectory.IndexOf("YolaGuide")] + "YolaGuide\\Image\\"; } }

        public static string URLYandexOrganization { get { return "https://yandex.ru/maps/org/{0}"; } }

        public static string URLYandexRoute { get { return "https://yandex.ru/maps/?rtext={0}&rtt=mt"; } }

        public static int NumberObjectsPerPage { get { return 6; } }

        public static List<long> Admins { get { return new() { 1059169240, 1802751981 }; } }

        public static Dictionary<long, Message> LastBotMsg = new();
    }
}
