using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class EmployeesController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly IStringLocalizer<EmployeesController> _localizer;

        public EmployeesController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI, IStringLocalizer<EmployeesController> localizer)
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
                await _httpService.CallMethod<ActionResult>(_appSettingsUI.CheckCompanyOwnerURL, HttpMethod.Get, token, new EmployeesViewModel { CompanyId = companyId, Id = user.Id});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Company");
            }

            ViewBag.CompanyId = companyId;   

            IEnumerable<EmployeesViewModel> employees = await _httpService.CallMethod<IEnumerable<EmployeesViewModel>>(_appSettingsUI.GetEmployeesURL, HttpMethod.Get, token, new EmployeesViewModel { CompanyId = companyId });

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> PartialAddEmployee()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                return PartialView("PartialAddEmployee");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeesViewModel employeesViewModel)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                if (employeesViewModel.Email.Trim() == user.Email.Trim())
                    return BadRequest(_localizer["CantAddOwner"]);

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.AddEmployeeURL, HttpMethod.Post, token, employeesViewModel);

                return Ok(_localizer["AddedEmployee"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEmployee(EmployeesViewModel employeesViewModel)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.RemoveEmployeeURL, HttpMethod.Delete, token, employeesViewModel);

                return Ok(_localizer["RemovedEmployee"]);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
