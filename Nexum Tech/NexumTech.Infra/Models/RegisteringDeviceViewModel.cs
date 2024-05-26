namespace NexumTech.Infra.Models
{
    public class RegisteringDeviceViewModel
    {
        public string description { get; set; } = "Temp Commands";
        public DataProvided dataProvided { get; set; } = new DataProvided();
        public Provider provider { get; set; } = new Provider();
    }

    public class DataProvided
    {
        public List<RegisteringEntity> entities { get; set; } = new List<RegisteringEntity>();
        public List<string> attrs { get; set; } = new List<string> { "on", "off" };
    }

    public class RegisteringEntity
    {
        public string id { get; set; }
        public string type { get; set; } = "Temp";
    }

    public class Provider
    {
        public RegisteringHttp http { get; set; } = new RegisteringHttp { url = "http://104.41.27.149:4041" };
        public bool legacyForwarding { get; set; } = true;
    }

    public class RegisteringHttp
    {
        public string url { get; set; }
    }

}
