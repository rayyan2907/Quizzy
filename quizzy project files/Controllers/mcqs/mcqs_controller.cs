using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers.quiz
{
    public class mcqController : Controller
    {
        public IActionResult attemptQuiz()
        {
            return View("make_quiz");
        }
    }
}
