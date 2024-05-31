namespace NexumTech.Infra.WEB.ViewModels
{
    public class RealTimeChartViewModel
    {
        public double Value { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public TimeInstant TimeInstant { get; set; }
    }

    public class TimeInstant
    {
        public DateTime Value { get; set; }
    }
}
