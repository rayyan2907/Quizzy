using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Models;
using System.Data;
using System.Globalization;

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

        [HttpPost]

        public IActionResult updatequiz(String id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            Console.WriteLine(id);

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            
            return RedirectToAction("quizMake");                
        }

        [HttpPost]   
        
        public IActionResult deletequiz(String id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(id);
            bool deleted = createQuizBL.deleteQuiz(id);
            if(deleted)
            {
                TempData["Check"] = "Quiz deleted successfully.";
            }
            else
            {
                TempData["log"] = "Cannot delete Quiz";
            }

            return RedirectToAction("quizMake");
        }

        public IActionResult assignquiz(String id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(id);

            bool assigned = createQuizBL.assignQuiz(id);
            if (assigned)
            {
                TempData["Check"] = "Quiz assigned successfully.";
            }
            else
            {
                TempData["log"] = "Cannot assign Quiz";
            }

            return RedirectToAction("quizMake");
        }

        public IActionResult unassignquiz(String id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(id);

            bool assigned = createQuizBL.unassignQuiz(id);
            if (assigned)
            {
                TempData["Check"] = "Quiz unassigned successfully.";
            }
            else
            {
                TempData["log"] = "Cannot unassign Quiz";
            }

            return RedirectToAction("quizMake");
        }
    }
}
