using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.checkQuiz
{
    public class checkQuizController : Controller
    {
        public IActionResult checkQuiz()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            return View("showAllQuizzes");
        }

        public IActionResult openQuiz()
        {
            Console.WriteLine("page  is worling"); 
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(subject.subjectID, subject.teacherID, subject.name);

            DataTable dt = checkQuizBL.showAllQuizzes(subject.subjectID);
            ViewBag.quiz = dt;
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            return View("showAllQuizzes");
        }
    }
}