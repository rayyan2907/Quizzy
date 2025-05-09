using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.student;
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
            Console.WriteLine("page  is working");

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
        [HttpGet]
        public IActionResult showQuizStudents(string id)
        {
            Console.WriteLine("quiz id is " + id);
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");


            quiz_model quiz = createQuizBL.getQuizObj(id);

            HttpContext.Session.SetObject("quizObj", quiz);

            if (teacher == null)
            {
                Console.WriteLine("Teacher session object is NULL");
            }
            else
            {
                Console.WriteLine("Teacher session object found: " + teacher.first_name);
            }
            if (teacher == null || subject==null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }

            DataTable dt = checkQuizBL.studentQuizzes(id);

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.quizAttempts = dt;
            return View("checkQuiz");
        }
        [HttpGet]
        public IActionResult SQCheck(string id)
        {
            Console.WriteLine("this is SQ check page for studentID = " + id);
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            var quiz = HttpContext.Session.GetObject<quiz_model>("quizObj");
            if (teacher == null || subject == null ||quiz==null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }


            Models.Buisness_Models.Student dt1 = StudentBL.getData(id);
            Console.WriteLine($"we have got student with name ", dt1.first_name, dt1.last_name);
            DataTable dt2 = checkQuizBL.AnswersOfStudent(quiz.quizID, id);

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.student = dt1;
            ViewBag.answers = dt2;
            return View("checkSQ");
        }
    }
}