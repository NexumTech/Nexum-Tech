using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Controllers
{
    public class SignupController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public SignupController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public IActionResult Index()
        {
            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signup(SignupViewModel signupViewModel)
        {
            try
            {
                await _httpService.CallMethod<ActionResult>(_appSettingsUI.SignupURL, HttpMethod.Post, null, signupViewModel);

                return Ok($"Welcome to the team, {signupViewModel.Username}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
