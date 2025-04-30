using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Models;

namespace Quizzy.Controllers.teacher
{
    public class teacherController : Controller
    {
        Teacher teacher = new Teacher();
        public IActionResult mainPage()
        {
            string id = HttpContext.Session.GetString("UserId");
            teacher.teachID = id;

            return View("mainpage_teacher");
        }
    }
}
