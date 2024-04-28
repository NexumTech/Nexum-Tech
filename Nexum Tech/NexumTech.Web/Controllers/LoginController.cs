using Microsoft.AspNetCore.Mvc;

namespace NexumTech.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email != null && password != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Usuário não encontrado ou senha incorreta";
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
