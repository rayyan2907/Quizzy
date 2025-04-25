using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Models;

namespace Quizzy.Controllers.login
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("login_page");
        }

        public IActionResult register()
        {
            return View("register_page");
        }


        [HttpPost]
        public IActionResult Index(login_models model)
        {
            TempData["Message"] = $"User with email {model.email} just tried to login ";

            return RedirectToAction("index");
        }
    }
}
