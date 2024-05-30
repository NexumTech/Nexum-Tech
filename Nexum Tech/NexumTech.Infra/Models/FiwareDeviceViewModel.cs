namespace NexumTech.Infra.Models
{
    public class FiwareDeviceViewModel
    {
        public int count { get; set; }
        public List<FiwareDevice> devices { get; set; }
    }

    public class FiwareDeviceAttribute
    {
        public string object_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class FiwareDeviceCommand
    {
        public string object_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class FiwareDevice
    {
        public string device_id { get; set; }
        public string service { get; set; }
        public string service_path { get; set; }
        public string entity_name { get; set; }
        public string entity_type { get; set; }
        public string transport { get; set; }
        public List<FiwareDeviceAttribute> attributes { get; set; }
        public List<string> lazy { get; set; }
        public List<FiwareDeviceCommand> commands { get; set; }
        public List<string> static_attributes { get; set; }
        public string protocol { get; set; }
    }
}
