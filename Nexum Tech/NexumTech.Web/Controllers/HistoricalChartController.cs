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

        public IActionResult Index()
        {
            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetHistoricalTemperature(string dateFrom, string dateTo, int hOffset = 0, int hLimit = 100)
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
    }
}
