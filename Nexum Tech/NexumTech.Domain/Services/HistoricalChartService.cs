using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.ViewModels;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Services
{
    public class HistoricalChartService : IHistoricalChartService
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsAPI _appSettingsAPI;

        public HistoricalChartService(BaseHttpService httpService, IOptions<AppSettingsAPI> appSettingsAPI)
        {
            _httpService = httpService;
            _appSettingsAPI = appSettingsAPI.Value;
        }

        public async Task<List<HistoricalChartViewModel.TemperatureRecord>> GetHistoricalTemperature(string dateFrom, string dateTo, string deviceName, int hOffset = 0, int hLimit = 100)
        {
            var token = "";

            DateTime fromDate = DateTime.Parse(dateFrom);
            DateTime toDate = DateTime.Parse(dateTo);

            toDate = toDate.Date.AddDays(1).AddSeconds(-1);
            dateFrom = fromDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            dateTo = toDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "fiware-service", "smart" },
                { "fiware-servicepath", "/" },
                { "accept", "*/*" },
                { "Accept-Encoding", "gzip, deflate, br" },
                { "Connection", "keep-alive" }
            };

            var baseUrl = _appSettingsAPI.Fiware.ApiFiwareHistoricalChartURL.Replace("device", deviceName.Trim());
            var uriBuilder = new UriBuilder(baseUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["dateFrom"] = dateFrom;
            query["dateTo"] = dateTo;
            query["hOffset"] = hOffset.ToString();
            query["hLimit"] = hLimit.ToString();

            uriBuilder.Query = query.ToString();

            string url = uriBuilder.ToString();

            var response = await _httpService.CallMethod<HistoricalChartViewModel.HistoricalApiResponse>(url, HttpMethod.Get, token, headers: headers, urlFiware: url);

            var temperatureRecords = response.ContextResponses[0].ContextElement.Attributes[0].Values.Select(v => new HistoricalChartViewModel.TemperatureRecord
            {
                Id = v.Id,
                RecvTime = v.RecvTime,
                AttrName = v.AttrName,
                AttrType = v.AttrType,
                AttrValue = v.AttrValue
            }).ToList();

            return temperatureRecords;
        }
    }
}