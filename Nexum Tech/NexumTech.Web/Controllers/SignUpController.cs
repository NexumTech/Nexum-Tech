using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<SignupController> _localizer;

        public SignupController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI, IStringLocalizer<SignupController> localizer)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
            _localizer = localizer;
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

                return Ok($"{_localizer["Welcome"]}, {signupViewModel.Username}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
