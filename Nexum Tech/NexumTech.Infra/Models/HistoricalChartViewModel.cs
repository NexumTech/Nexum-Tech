namespace NexumTech.Infra.ViewModels
{
    public class HistoricalChartViewModel
    {
        public List<TemperatureRecord>? TemperatureRecords { get; set; }

        public class HistoricalApiResponse
        {
            public List<ContextResponse> ContextResponses { get; set; }
        }

        public class ContextResponse
        {
            public ContextElement ContextElement { get; set; }
        }

        public class ContextElement
        {
            public List<Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            public string Name { get; set; }
            public List<Value> Values { get; set; }
        }

        public class Value
        {
            public string Id { get; set; }
            public DateTime RecvTime { get; set; }
            public string AttrName { get; set; }
            public string AttrType { get; set; }
            public double AttrValue { get; set; }
        }

        public class TemperatureRecord
        {
            public string Id { get; set; }
            public DateTime RecvTime { get; set; }
            public string AttrName { get; set; }
            public string AttrType { get; set; }
            public double AttrValue { get; set; }
        }

        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DeviceName { get; set; }
        public int HOffset { get; set; }
        public int HLimit { get; set; }

    }
}
