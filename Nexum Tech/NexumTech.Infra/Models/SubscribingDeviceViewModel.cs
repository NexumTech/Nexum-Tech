namespace NexumTech.Infra.Models
{
    public class SubscribingDeviceViewModel
    {
        public string description { get; set; } = "Notify STH-Comet of all Motion Sensor count changes";
        public Subject subject { get; set; } = new Subject();
        public Notification notification { get; set; } = new Notification();
    }

    public class Subject
    {
        public List<SubscribingEntity> entities { get; set; } = new List<SubscribingEntity>();
        public Condition condition { get; set; } = new Condition { attrs = new List<string> { "temperature" } };
    }

    public class SubscribingEntity
    {
        public string id { get; set; }
        public string type { get; set; } = "Temp";
    }

    public class Condition
    {
        public List<string> attrs { get; set; }
    }

    public class Notification
    {
        public SubscribingHttp http { get; set; } = new SubscribingHttp { url = "http://104.41.27.149:8666/notify" };
        public List<string> attrs { get; set; } = new List<string> { "temperature" };
        public string attrsFormat { get; set; } = "legacy";
    }

    public class SubscribingHttp
    {
        public string url { get; set; }
    }
}
