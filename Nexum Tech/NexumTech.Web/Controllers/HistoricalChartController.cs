using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;

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
        public async Task<ActionResult> GetHistoricalTemperature()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                HistoricalChartViewModel temperature = await _httpService.CallMethod<HistoricalChartViewModel>(_appSettingsUI.Fiware.ApiFiwareHistoricalChartURL, HttpMethod.Get, token, headers: headers, urlFiware:_appSettingsUI.Fiware.ApiFiwareHistoricalChartURL);

                return Ok(temperature);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
