using Microsoft.AspNetCore.Mvc;
using NexumTech.Web.Controllers.Filters;
using NexumTech.Web.Services;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class RealTimeChartController : Controller
    {
        private readonly IRealTimeChartService _realTimeChartService;

        public RealTimeChartController(IRealTimeChartService historicalChartService)
        {
            _realTimeChartService = historicalChartService;
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
                var temperature = await _realTimeChartService.GetRealTemperatureAsync(token);
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
