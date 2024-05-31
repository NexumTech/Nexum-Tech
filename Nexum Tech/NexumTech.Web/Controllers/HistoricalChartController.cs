// Web/Controllers/HistoricalChartController.cs
using Microsoft.AspNetCore.Mvc;
using NexumTech.Infra.ViewModels;
using NexumTech.Web.Services;

namespace NexumTech.Web.Controllers
{
    public class HistoricalChartController : Controller
    {
        private readonly IHistoricalChartService _historicalChartService;

        public HistoricalChartController(IHistoricalChartService historicalChartService)
        {
            _historicalChartService = historicalChartService;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt"];

            UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

            IEnumerable<CompanyViewModel> companies = await _httpService.CallMethod<IEnumerable<CompanyViewModel>>(_appSettingsUI.GetCompaniesURL, HttpMethod.Get, token, user);

            ViewBag.CurrentCompanyId = companies != null ? companies.FirstOrDefault().Id : 0;

            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoricalTemperature(string dateFrom, string dateTo, string deviceName, int hOffset = 0, int hLimit = 100)
        {
            try
            {
                var temperatureRecords = await _historicalChartService.GetHistoricalTemperature(dateFrom, dateTo, hOffset, hLimit);

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
