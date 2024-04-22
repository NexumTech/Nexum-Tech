using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nexum_Tech.Domain.Interfaces;
using Nexum_Tech.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nexum_Tech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITest _test;

        public HomeController(ILogger<HomeController> logger, ITest test)
        {
            _logger = logger;
            _test = test;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Teste = _test.Teste();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
