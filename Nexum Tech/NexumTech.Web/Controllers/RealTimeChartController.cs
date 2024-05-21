using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Controllers
{
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
        //[Authorize]
        public async Task<ActionResult> GetRealTemperature()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                RealTimeChartViewModel temperature = await _httpService.CallMethod<RealTimeChartViewModel>(_appSettingsUI.Fiware.ApiFiwareBaseURL, HttpMethod.Get, token, headers: headers, urlFiware:_appSettingsUI.Fiware.ApiFiwareBaseURL);

                return Ok(temperature);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
