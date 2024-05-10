using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;

namespace NexumTech.Web.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly BaseHttpService _httpService;
        private readonly AppSettingsWEB _appSettingsUI;

        public PasswordResetController(BaseHttpService baseHttpService, IOptions<AppSettingsWEB> appSettingsUI) 
        {
            _httpService = baseHttpService;
            _appSettingsUI = appSettingsUI.Value;
        }

        public async Task<IActionResult> Index(string token)
        {
            try
            {
                await _httpService.CallMethod<ActionResult>(_appSettingsUI.PasswordResetTokenValidationURL, HttpMethod.Get, token, null);

                var currentTheme = Request.Cookies["CurrentTheme"];
                ViewBag.CurrentTheme = currentTheme;

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                };

                Response.Cookies.Append("passwordResetJwt", token, cookieOptions);

                ViewBag.TokenExpired = false;

                return View();
            } 
            catch (Exception)
            {
                ViewBag.TokenExpired = true;

                return View();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword(PasswordResetViewModel passwordResetViewModel)
        {
            try
            {
                var token = Request.Cookies["passwordResetJwt"];

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.UpdatePasswordURL, HttpMethod.Put, token, passwordResetViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
