using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;
using NexumTech.Infra.WEB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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

        public IActionResult Index()
        {
            var token = Request.Cookies["jwt"];

            return View();
        }
    }
}