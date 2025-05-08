using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.attemptQuiz
{
    public class quizAttemptController : Controller
    {
        [HttpGet]
        public IActionResult attemptQuiz(string quizId)
        {
            Console.WriteLine("we have quiz id " + quizId + "to solve");
            var student = HttpContext.Session.GetObject<Student>("StudentObj");

            if (student == null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }
           
            quiz_model q = new quiz_model();
            q = createQuizBL.getQuizObj(quizId);
            Console.WriteLine($"A new object of quiz {q.quizName} and time is {q.given_time} has been created");

            HttpContext.Session.SetObject("quizObj", q);

            string id = q.subID;
            subject_model sub = subjectBL.getSubfromid(id);
            HttpContext.Session.SetObject("subjectObj", sub);
            Console.WriteLine($"course id is {id}");

            Console.WriteLine($"student with name {student.first_name} {student.last_name} is going to attempt the quiz {q.quizID} and {q.quizName} of {sub.name}");
            ViewBag.quiz = q;   
            ViewBag.stu = student;
            ViewBag.sub = sub;
            return View("attemptQuiz");
        }

        public IActionResult attempt()
        {
            var student = HttpContext.Session.GetObject<Student>("StudentObj");
            var sub = HttpContext.Session.GetObject<subject_model>("subjectObj");
            quiz_model quiz = new quiz_model();
            quiz = HttpContext.Session.GetObject<quiz_model>("quizObj");
            
            if (student == null || sub == null || quiz == null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");

            }

            Console.WriteLine($"A new object of quiz {quiz.quizName} and time is {quiz.given_time} has been fetched");

            DataTable dt = createQuizBL.getMcqs(quiz);
            DataTable dt2 = createQuizBL.getShqs(quiz);

            Console.WriteLine("rows are"+dt.Rows.Count);
            ViewBag.QuizData = quiz;
            ViewBag.mcqs = dt;
            ViewBag.sub = sub;
            ViewBag.stu = student;
            ViewBag.shq = dt2;

            return View("attemptPage");
        }
    }
}
