namespace NexumTech.Infra.Models
{
    public class ProvisioningDeviceViewModel
    {
        public List<Device> devices { get; set; }
    }

    public class Device
    {
        public string device_id { get; set; }
        public string entity_name { get; set; }
        public string entity_type { get; set; } = "Temp";
        public string protocol { get; set; } = "PDI-IoTA-UltraLight";
        public string transport { get; set; } = "MQTT";
        public List<Command> commands { get; set; } = new List<Command>
    {
        new Command { name = "on", type = "command" },
        new Command { name = "off", type = "command" }
    };
        public List<Attribute> attributes { get; set; } = new List<Attribute>
    {
        new Attribute { object_id = "s", name = "state", type = "Text" },
        new Attribute { object_id = "t", name = "temperature", type = "Float" }
    };
    }

    public class Command
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Attribute
    {
        public string object_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }
}
