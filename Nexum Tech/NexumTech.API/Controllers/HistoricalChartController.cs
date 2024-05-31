using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Infra.ViewModels;
using NexumTech.Web.Services;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HistoricalChartController : Controller
    {
        private readonly IHistoricalChartService _historicalChartService;

        public HistoricalChartController(IHistoricalChartService historicalChartService)
        {
            _historicalChartService = historicalChartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetHistoricalTemperature(HistoricalChartViewModel historicalChartViewModel)
        {
            try
            {                
                return Ok(await _historicalChartService.GetHistoricalTemperature(historicalChartViewModel.DateFrom, historicalChartViewModel.DateTo, historicalChartViewModel.DeviceName, historicalChartViewModel.HOffset, historicalChartViewModel.HLimit));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
