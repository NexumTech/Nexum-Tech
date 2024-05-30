
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB.ViewModels;
using NexumTech.Infra.WEB;
using NexumTech.Web.Services;

public class RealTimeChartService : IRealTimeChartService
{
    private readonly BaseHttpService _httpService;
    private readonly AppSettingsWEB _appSettingsUI;

    public RealTimeChartService(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
    {
        _httpService = httpService;
        _appSettingsUI = appSettingsUI.Value;
    }

    public async Task<RealTimeChartViewModel> GetRealTemperatureAsync(string token)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "fiware-service", "smart" },
            { "fiware-servicepath", "/" },
            { "accept", "application/json" }
        };

        return await _httpService.CallMethod<RealTimeChartViewModel>(_appSettingsUI.Fiware.ApiFiwareRealTimeChartURL, HttpMethod.Get, token, headers: headers, urlFiware: _appSettingsUI.Fiware.ApiFiwareRealTimeChartURL);
    }
}