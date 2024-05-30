﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;
using System.ComponentModel.Design;
using System.Reflection.PortableExecutable;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class CompanyController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly IStringLocalizer<CompanyController> _localizer;

        public CompanyController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI, IStringLocalizer<CompanyController> localizer)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            var token = Request.Cookies["jwt"];

            UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

            CompanyViewModel company = await _httpService.CallMethod<CompanyViewModel>(_appSettingsUI.GetCompanyURL, HttpMethod.Get, token, new CompanyViewModel { OwnerId = user.Id });
      
            return View(company);
        }

        [HttpGet]
        public async Task<IActionResult> PartialCreateCompany()
        {
            try
            {               
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                CompanyViewModel company = await _httpService.CallMethod<CompanyViewModel>(_appSettingsUI.GetCompanyURL, HttpMethod.Get, token, new CompanyViewModel { OwnerId = user.Id });

                return PartialView("PartialCreateCompany", company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromForm] CompanyViewModel company)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                company.OwnerId = user.Id;

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.CreateCompanyURL, HttpMethod.Post, token, company);

                return Ok(_localizer["CreatedCompany"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromForm] CompanyViewModel company)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                company.OwnerId = user.Id;

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.UpdateCompanyURL, HttpMethod.Put, token, company);

                return Ok(_localizer["UpdatedCompany"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> CloseCompany()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                CompanyViewModel company = await _httpService.CallMethod<CompanyViewModel>(_appSettingsUI.GetCompanyURL, HttpMethod.Get, token, new CompanyViewModel { OwnerId = user.Id });

                IEnumerable<DevicesViewModel> devices = await _httpService.CallMethod<IEnumerable<DevicesViewModel>>(_appSettingsUI.GetDevicesURL, HttpMethod.Get, token, new DevicesViewModel { CompanyId = company.Id });

                #region Building fiware parameters
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("fiware-service", "smart");
                headers.Add("fiware-servicepath", "/");
                headers.Add("accept", "application/json");
                #endregion

                #region Fiware - remove device
                foreach(DevicesViewModel device in devices)
                {
                    try
                    {
                        await _httpService.CallMethod<dynamic>(String.Empty, HttpMethod.Delete, token, null, headers, $"{_appSettingsUI.Fiware.ApiFiwareRemoveDeviceURL}{device.Name}");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(_localizer["RemoveDeviceError"]);
                    }
                }
                #endregion

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.DeleteCompanyURL, HttpMethod.Delete, token, new CompanyViewModel { OwnerId = user.Id });

                return Ok(_localizer["DeletedCompany"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
