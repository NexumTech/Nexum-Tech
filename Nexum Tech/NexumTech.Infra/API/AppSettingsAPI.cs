namespace NexumTech.Infra.API
{
    public class AppSettingsAPI
    {
        public JWT JWT { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
    }
}
