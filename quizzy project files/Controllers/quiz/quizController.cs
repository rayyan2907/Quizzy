using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.quiz
{
    public class quizController : Controller
    {
        public IActionResult showQuiz()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");

            if (stu == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }
            string id = HttpContext.Session.GetString("courseID");

            if (string.IsNullOrEmpty(id))
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }

            subject_model sub = subjectBL.getSubfromid(id);

            Console.WriteLine($"course id is {id}");

            Console.WriteLine($"student with name {stu.first_name} {stu.last_name} is opening the quizes of the course {sub.name}");

            DataTable dt = stu_quizBL.getquiz(sub.subjectID);

            ViewBag.quiz =  dt;

            ViewBag.stu = stu;
            ViewBag.sub = sub;
            return View("~/Views/student/showQuizes.cshtml");
        }



        public IActionResult showAttempts()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");

            if (stu == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }
            string id = HttpContext.Session.GetString("courseID");

            if (string.IsNullOrEmpty(id))
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }

            subject_model sub = subjectBL.getSubfromid(id);

            Console.WriteLine($"course id is {id}");

            Console.WriteLine($"student with name {stu.first_name} {stu.last_name} is opening the quiz attempts of the course {sub.name}");


            ViewBag.stu = stu;
            ViewBag.sub = sub;
            return View("~/Views/student/showAttemptedQuizes.cshtml");
        }
    }
}
