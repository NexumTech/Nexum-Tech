using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DevicesController : Controller
    {
        private readonly IDevicesService _devicesService;

        public DevicesController(IDevicesService devicesService) 
        {
            _devicesService = devicesService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DevicesViewModel>>> GetDevices(DevicesViewModel devicesViewModel)
        {
            try
            {
                return Ok(await _devicesService.GetDevices(devicesViewModel.CompanyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                await _devicesService.CreateDevice(devicesViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> RemoveDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                await _devicesService.RemoveDevice(devicesViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
