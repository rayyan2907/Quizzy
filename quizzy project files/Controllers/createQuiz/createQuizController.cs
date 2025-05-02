using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.createQuiz
{
    public class createQuizController : Controller
    {
        public IActionResult quizMake()
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine($"{subject.name}");

            DataTable dt = getQuizBL.getQuiz(subject.subjectID);

            ViewBag.quiz = dt;
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            return View("showQuiz");
        }

        
    }
}
