using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.result
{
    public class resultController : Controller
    {
        public IActionResult showResults()
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
            Console.WriteLine($"student with name {stu.first_name} {stu.last_name} is opening the reuslts of the course {sub.name}");

            DataTable dt = stu_quizBL.getresults(sub.subjectID);
            ViewBag.result = dt;
            ViewBag.stu = stu;
            ViewBag.sub = sub;
            return View("~/Views/student/showResults.cshtml");
        }

        public IActionResult openQuizResults()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");

            if (stu == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            DataTable dt = stu_quizBL.getresultsOpenQuiz(stu.stuID);
            ViewBag.result = dt;
            ViewBag.stu = stu;
            return View("~/Views/student/openQuizResults.cshtml");
        }
    }
}
