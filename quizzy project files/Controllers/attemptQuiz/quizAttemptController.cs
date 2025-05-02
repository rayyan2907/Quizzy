using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers.attemptQuiz
{
    public class quizAttemptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
