using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;
using System.Web;

namespace NexumTech.Web.Controllers
{
    public class HistoricalChartController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public HistoricalChartController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoricalTemperature(string dateFrom, string dateTo, int hOffset = 0, int hLimit = 100)
        {
            try
            {
                var token = Request.Cookies["jwt"];

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

                var baseUrl = _appSettingsUI.Fiware.ApiFiwareHistoricalChartURL;
                var uriBuilder = new UriBuilder(baseUrl);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["dateFrom"] = dateFrom;
                query["dateTo"] = dateTo;
                query["hOffset"] = hOffset.ToString();
                query["hLimit"] = hLimit.ToString();

                uriBuilder.Query = query.ToString();

                string url = uriBuilder.ToString();

                var response = await _httpService.CallMethod<HistoricalApiResponse>(url, HttpMethod.Get, token, headers: headers, urlFiware: url);

                var temperatureRecords = response.ContextResponses[0].ContextElement.Attributes[0].Values.Select(v => new TemperatureRecord
                {
                    Id = v.Id,
                    RecvTime = v.RecvTime,
                    AttrName = v.AttrName,
                    AttrType = v.AttrType,
                    AttrValue = v.AttrValue
                }).ToList();

                var viewModel = new HistoricalChartViewModel
                {
                    TemperatureRecords = temperatureRecords
                };

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

    }
}
