using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Infra.WEB.ViewModels;
using NexumTech.Web.Services;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RealTimeChartController : Controller
    {
        private readonly IRealTimeChartService _realTimeChartService;

        public RealTimeChartController(IRealTimeChartService realTimeChartService)
        {
            _realTimeChartService = realTimeChartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetRealTemperature(RealTimeChartViewModel realTimeChartViewModel)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                return Ok(await _realTimeChartService.GetRealTemperatureAsync(realTimeChartViewModel.DeviceName, token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
