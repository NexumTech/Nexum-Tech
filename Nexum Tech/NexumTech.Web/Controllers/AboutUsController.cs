using Microsoft.AspNetCore.Mvc;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    [JwtAuthentication]
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
