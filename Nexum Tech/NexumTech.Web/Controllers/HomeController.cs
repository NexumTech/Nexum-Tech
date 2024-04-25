using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using System.Diagnostics;

namespace NexumTech.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettingsWEB _appSettingsUI;
        private readonly BaseHttpService _httpService;

        public HomeController(BaseHttpService httpService, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _appSettingsUI = appSettingsUI.Value;
            _httpService = httpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var parameters = new TestViewModel { Name = "Testing" };
            var data = await _httpService.CallMethod<Object>(_appSettingsUI.EndpointTeste, HttpMethod.Get, parameters);
            ViewBag.Teste = data;
            return View();
        }
    }
}