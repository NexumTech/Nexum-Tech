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
                var temperature = await _realTimeChartService.GetRealTemperatureAsync(token);
                return Ok(temperature);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
