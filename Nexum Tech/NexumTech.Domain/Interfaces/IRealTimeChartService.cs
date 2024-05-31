using NexumTech.Infra.WEB.ViewModels;

namespace NexumTech.Web.Services
{
    public interface IRealTimeChartService
    {
        Task<RealTimeChartViewModel> GetRealTemperatureAsync(string deviceName, string token);
    }
}