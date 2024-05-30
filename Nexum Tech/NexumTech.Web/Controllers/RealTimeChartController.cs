using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class RealTimeChartController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public RealTimeChartController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRealTemperature()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                RealTimeChartViewModel temperature = await _httpService.CallMethod<RealTimeChartViewModel>(_appSettingsUI.Fiware.ApiFiwareRealTimeChartURL, HttpMethod.Get, token, headers: headers, urlFiware:_appSettingsUI.Fiware.ApiFiwareRealTimeChartURL);

                return Ok(temperature);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
