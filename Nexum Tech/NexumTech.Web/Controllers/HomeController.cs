using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class HomeController : Controller
    {
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly BaseHttpService _httpService;

        public HomeController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _appSettingsUI = appSettingsUI.Value;
            _httpService = httpService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                return View();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}