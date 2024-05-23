public class HistoricalChartViewModel
{
    public List<TemperatureRecord> TemperatureRecords { get; set; }
}

public class TemperatureRecord
{
    public string Id { get; set; }
    public DateTime RecvTime { get; set; }
    public string AttrName { get; set; }
    public string AttrType { get; set; }
    public double AttrValue { get; set; }
}
