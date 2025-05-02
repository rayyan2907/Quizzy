using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers.createQuiz
{
    public class createQuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
