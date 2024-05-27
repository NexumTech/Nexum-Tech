using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;
using System.Reflection.PortableExecutable;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class DevicesController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly IStringLocalizer<DevicesController> _localizer;

        public DevicesController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI, IStringLocalizer<DevicesController> localizer)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(int companyId)
        {
            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            var token = Request.Cookies["jwt"];

            UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

            try
            {
                await _httpService.CallMethod<ActionResult>(_appSettingsUI.CheckCompanyOwnerURL, HttpMethod.Get, token, new EmployeesViewModel { CompanyId = companyId, Id = user.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Company");
            }

            IEnumerable<DevicesViewModel> devices = await _httpService.CallMethod<IEnumerable<DevicesViewModel>>(_appSettingsUI.GetDevicesURL, HttpMethod.Get, token, new DevicesViewModel { CompanyId = companyId });

            ViewBag.CompanyId = companyId;

            return View(devices);
        }

        [HttpGet]
        public async Task<IActionResult> PartialCreateDevice()
        {
            try
            {
                var currentTheme = Request.Cookies["CurrentTheme"];
                ViewBag.CurrentTheme = currentTheme;

                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                return PartialView("PartialCreateDevice");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                #region Building fiware parameters

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");

                ProvisioningDeviceViewModel provisioningDeviceViewModel = new ProvisioningDeviceViewModel();
                provisioningDeviceViewModel.devices = new List<Device>();
                provisioningDeviceViewModel.devices.Add(new Device
                {
                    device_id = devicesViewModel.Name.Trim(),
                    entity_name = $"urn:ngsi-ld:{devicesViewModel.Name.Trim()}",
                });

                RegisteringDeviceViewModel registeringDeviceViewModel = new RegisteringDeviceViewModel();
                DataProvided dataProvided = new DataProvided();
                RegisteringEntity entity = new RegisteringEntity { id = $"urn:ngsi-ld:{devicesViewModel.Name.Trim()}" };
                dataProvided.entities = new List<RegisteringEntity>();
                dataProvided.entities.Add(entity);
                registeringDeviceViewModel.dataProvided = dataProvided;

                SubscribingDeviceViewModel subscribingDeviceViewModel = new SubscribingDeviceViewModel();
                Subject subject = new Subject();
                SubscribingEntity subscribingEntity = new SubscribingEntity { id = $"urn:ngsi-ld:{devicesViewModel.Name.Trim()}" };
                subject.entities = new List<SubscribingEntity>();
                subject.entities.Add(subscribingEntity);
                subscribingDeviceViewModel.subject = subject;

                #endregion

                #region Fiware - provisioning device
                try
                { 
                    await _httpService.CallMethod<dynamic>(String.Empty, HttpMethod.Post, token, provisioningDeviceViewModel, headers, _appSettingsUI.Fiware.ApiFiwareProvisioningDeviceURL);
                }
                catch (Exception ex)
                {
                    return BadRequest(_localizer["ProvisioningDeviceError"]);
                }
                #endregion

                #region Fiware - registering device
                try
                {
                    await _httpService.CallMethod<dynamic>(String.Empty, HttpMethod.Post, token, registeringDeviceViewModel, headers, _appSettingsUI.Fiware.ApiFiwareRegisteringDeviceURL);
                }
                catch (Exception ex)
                {
                    return BadRequest(_localizer["RegisteringDeviceError"]);
                }
                #endregion

                #region Fiware - subscribing device
                try
                {
                    await _httpService.CallMethod<dynamic>(String.Empty, HttpMethod.Post, token, subscribingDeviceViewModel, headers, _appSettingsUI.Fiware.ApiFiwareSubscribingDeviceURL);
                }
                catch (Exception ex)
                {
                    return BadRequest(_localizer["SubscribingDeviceError"]);
                }
                #endregion

                await _httpService.CallMethod<UserViewModel>(_appSettingsUI.CreateDeviceURL, HttpMethod.Post, token, devicesViewModel);

                return Ok(_localizer["CreatedDevice"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                #region Building fiware parameters
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");
                #endregion

                #region Fiware - remove device
                try
                {
                    await _httpService.CallMethod<dynamic>(String.Empty, HttpMethod.Delete, token, null, headers, $"{_appSettingsUI.Fiware.ApiFiwareRemoveDeviceURL}{devicesViewModel.Name.Trim()}");
                }
                catch (Exception ex)
                {
                    return BadRequest(_localizer["RemoveDeviceError"]);
                }
                #endregion

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.RemoveDeviceURL, HttpMethod.Delete, token, devicesViewModel);

                return Ok(_localizer["RemovedDevice"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
