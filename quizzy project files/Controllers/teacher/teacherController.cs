using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers.teacher
{
    public class teacherController : Controller
    {
        public IActionResult mainPage()
        {
            return View("mainpage_teacher");
        }
    }
}
