using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.ViewModels;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;
using NexumTech.Web.Services;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class HistoricalChartController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public HistoricalChartController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];

            UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

            IEnumerable<CompanyViewModel> companies = await _httpService.CallMethod<IEnumerable<CompanyViewModel>>(_appSettingsUI.GetCompaniesURL, HttpMethod.Get, token, user);

            ViewBag.CurrentCompanyId = companies.Count() > 0 ? companies.FirstOrDefault().Id : 0;

            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoricalTemperature(string dateFrom, string dateTo, string deviceName, int hOffset = 0, int hLimit = 100)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                List<HistoricalChartViewModel.TemperatureRecord> temperatureRecords = await _httpService.CallMethod<List<HistoricalChartViewModel.TemperatureRecord>>(_appSettingsUI.GetHistoricalTemperatureURL, HttpMethod.Get, token, new HistoricalChartViewModel { DateFrom = dateFrom, DateTo = dateTo, DeviceName = deviceName, HOffset = hOffset, HLimit = hLimit});

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
