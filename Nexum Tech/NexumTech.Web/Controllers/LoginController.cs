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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                string userValidated = await _httpService.CallMethod<string>(_appSettingsUI.LoginURL, HttpMethod.Get, new LoginViewModel { Email = email, Password = password });
                if (int.TryParse(userValidated, out int validated) && validated >= 1)
                {
                    ViewBag.userValidated = true;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.userValidated = false;
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.userValidated = false;
                ViewBag.TestResponse = "Erro ao fazer a solicitação para o endpoint de teste da API: " + ex.Message;
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
