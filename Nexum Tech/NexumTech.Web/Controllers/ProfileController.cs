using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using NexumTech.Web.Controllers.Filters;
using System.Runtime.CompilerServices;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class ProfileController : Controller
    {
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly BaseHttpService _httpService;

        public ProfileController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _appSettingsUI = appSettingsUI.Value;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> PartialProfileSettings()
        {
            try
            {
                var currentTheme = Request.Cookies["CurrentTheme"];
                ViewBag.CurrentTheme = currentTheme;

                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                ProfileViewModel profileViewModel = new ProfileViewModel
                {
                    User = user,
                    Base64Photo = user.Photo != null ? ConvertToDataUrl(user.Photo, "image/jpeg") : "",
                };

                return PartialView("PartialProfileSettings", profileViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel profileViewModel)
        {
            try
            {
                var token = Request.Cookies["jwt"];

                UserViewModel user = await _httpService.CallMethod<UserViewModel>(_appSettingsUI.GetUserInfoURL, HttpMethod.Get, token);

                if (String.IsNullOrEmpty(profileViewModel.User.Username))
                    profileViewModel.User.Username = user.Username;

                await _httpService.CallMethod<ActionResult>(_appSettingsUI.UpdateProfileURL, HttpMethod.Put, token, profileViewModel);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static string ConvertToDataUrl(byte[] bytes, string mimeType)
        {
            string base64String = Convert.ToBase64String(bytes);

            return $"data:{mimeType};base64,{base64String}";
        }
    }
}
