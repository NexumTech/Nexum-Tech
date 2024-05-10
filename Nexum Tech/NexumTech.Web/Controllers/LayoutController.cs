using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class LayoutController : Controller
    {
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly BaseHttpService _httpService;

        public LayoutController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _appSettingsUI = appSettingsUI.Value;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                LayoutViewModel layoutViewModel = new LayoutViewModel
                {
                    User = user,
                    Base64Photo = ConvertToDataUrl(user.Photo, "image/jpeg"),
                };

                return Ok(layoutViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append("Culture", culture, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Ok();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Login");
        }

        private static string ConvertToDataUrl(byte[] bytes, string mimeType)
        {
            string base64String = Convert.ToBase64String(bytes);

            return $"data:{mimeType};base64,{base64String}";
        }
    }
}
