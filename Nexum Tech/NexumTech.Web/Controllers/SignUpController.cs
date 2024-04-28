using Microsoft.AspNetCore.Mvc;

namespace NexumTech.Web.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp(string name, string username, string email, string password)
        {
            //próximo passo: BD
            if (name != null && username != null && email != null && password != null)
            {
                //HttpContext.Session.SetString("Logado", "true");
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.ErrorMessage = "Usuário não encontrado";
                return RedirectToAction("Index", "SignUp");
            }
        }
    }
}
