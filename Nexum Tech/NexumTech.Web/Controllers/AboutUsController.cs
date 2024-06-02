using Microsoft.AspNetCore.Mvc;

namespace NexumTech.Web.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
