using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public LoginController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpService = httpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public IActionResult Index(int requestType = 0)
        {
            if (requestType == 1) TempData["AuthenticationMessage"] = true;

            var currentTheme = Request.Cookies["CurrentTheme"];
            ViewBag.CurrentTheme = currentTheme;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var token =  await _httpService.CallMethod<string>(_appSettingsUI.AuthenticateURL, HttpMethod.Get, null, loginViewModel);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
