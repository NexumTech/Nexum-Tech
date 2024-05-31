using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB.ViewModels;
using NexumTech.Infra.WEB;
using NexumTech.Web.Services;

public class RealTimeChartService : IRealTimeChartService
{
    private readonly BaseHttpService _httpService;
    private readonly AppSettingsAPI _appSettingsAPI;

    public RealTimeChartService(BaseHttpService httpService, IOptions<AppSettingsAPI> appSettingsAPI)
    {
        _httpService = httpService;
        _appSettingsAPI = appSettingsAPI.Value;
    }

    public async Task<RealTimeChartViewModel> GetRealTemperatureAsync(string deviceName, string token)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "fiware-service", "smart" },
            { "fiware-servicepath", "/" },
            { "accept", "application/json" }
        };

        return await _httpService.CallMethod<RealTimeChartViewModel>(_appSettingsAPI.Fiware.ApiFiwareRealTimeChartURL.Replace("device", deviceName.Trim()), HttpMethod.Get, token, headers: headers, urlFiware: _appSettingsAPI.Fiware.ApiFiwareRealTimeChartURL.Replace("device", deviceName.Trim()));
    }
}