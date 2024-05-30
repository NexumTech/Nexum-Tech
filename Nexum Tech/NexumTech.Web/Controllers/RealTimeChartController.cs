using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
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

        public async Task<IActionResult> Index()
        {
            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            var token = Request.Cookies["jwt"];

            UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

            IEnumerable<CompanyViewModel> companies = await _httpService.CallMethod<IEnumerable<CompanyViewModel>>(_appSettingsUI.GetCompaniesURL, HttpMethod.Get, token, user);

            ViewBag.CurrentCompanyId = companies != null ? companies.FirstOrDefault().Id : 0;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRealTemperature(string deviceName)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                RealTimeChartViewModel temperature = await _httpService.CallMethod<RealTimeChartViewModel>(_appSettingsUI.Fiware.ApiFiwareRealTimeChartURL.Replace("device", deviceName.Trim()), HttpMethod.Get, token, headers: headers, urlFiware:_appSettingsUI.Fiware.ApiFiwareRealTimeChartURL.Replace("device", deviceName.Trim()));

                return Ok(temperature);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDevices(int companyId)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                IEnumerable<DevicesViewModel> devices = await _httpService.CallMethod<IEnumerable<DevicesViewModel>>(_appSettingsUI.GetDevicesURL, HttpMethod.Get, token, new DevicesViewModel { CompanyId = companyId });

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
