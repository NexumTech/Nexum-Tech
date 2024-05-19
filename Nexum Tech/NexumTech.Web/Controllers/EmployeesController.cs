using Microsoft.AspNetCore.Mvc;
using NexumTech.Web.Controllers.Filters;

namespace NexumTech.Web.Controllers
{
    public class EmployeesController : Controller
    {
        [JwtAuthentication]
        public IActionResult Index()
        {
            return View();
        }
    }
}
