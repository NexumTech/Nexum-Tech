namespace NexumTech.Infra.API
{
    public class AppSettingsAPI
    {
        public JWT JWT { get; set; }
        public SMTP SMTP { get; set; }
        public Fiware Fiware { get; set; }
        public string PasswordResetURL { get; set; }
    }

    public class SMTP
    {
        public string Username { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
    }

    public class Fiware
    {
        public string ApiFiwareRealTimeChartURL { get; set; }
        public string ApiFiwareHistoricalChartURL { get; set; }
    }
}
