using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Models;

namespace Quizzy.Controllers.student
{
    public class studentController : Controller
    {
        public IActionResult mainPage()
        {
            string id = HttpContext.Session.GetString("UserId");

            return View("mainPage_student", id);
        }


    }
}
