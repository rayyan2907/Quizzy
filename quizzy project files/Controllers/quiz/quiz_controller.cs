using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers.quiz
{
    public class quizController : Controller
    {
        public IActionResult attemptQuiz()
        {
            return View("make_quiz");
        }
    }
}
