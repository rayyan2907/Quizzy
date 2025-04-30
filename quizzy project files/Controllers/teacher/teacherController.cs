using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Layer.teacher;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.subjects;
using System.Data;

namespace Quizzy.Controllers.teacher
{
    public class teacherController : Controller
    {
        Teacher teacher = new Teacher();

        //there is an issue in saving the session of the teacher
        public IActionResult home()
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            viewModel view = new viewModel
            {
                Teacher = teacher,
                Subject = subject
            };
            return View("mainpage_teacher", view);
        }
        public IActionResult main()
        {
            string id = HttpContext.Session.GetString("teacId");
            Console.WriteLine(id);
            if (string.IsNullOrEmpty(id))
            {
                // You can reload from cookie or redirect to login
                if (Request.Cookies["teacId"] != null)
                {

                    id = Request.Cookies["teacId"];
                    HttpContext.Session.SetString("teacId", id); // restore session
                    Console.WriteLine($"an old session of teacher with id {id} was found ");
                }
                else
                {
                    TempData["log"] = "Session not found";

                    return RedirectToAction("index", "login");
                }
            }

            teacher = teacherBL.getData(id);
            HttpContext.Session.SetObject("teacherObj", teacher);
            return RedirectToAction("subjectSelect");
        }
        [HttpGet]
        public IActionResult subjectSelect()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            if (teacher == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            DataTable dt = getSubject.getSub(teacher.teachID);
            ViewBag.Subjects = dt;
            return View("selectSub");
        }


        [HttpPost]
        public IActionResult subjectSelect(string selectedSubjectId)
        {
            // Store in session
            HttpContext.Session.SetString("subjectID", selectedSubjectId);


            subject_model model = new subject_model();
            model = subjectBL.getSubfromid(selectedSubjectId);
            HttpContext.Session.SetObject("subjectObj", model);

            return RedirectToAction("home");
        }

        public IActionResult logOut()
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            if ( teacher == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine($"user with name {teacher.first_name} {teacher.last_name} is loging out");
            HttpContext.Session.Clear();

            teacher = null;
            if (Request.Cookies["teacId"] != null)
            {
                Response.Cookies.Delete("teacId");
            }

            TempData["check"] = "Logged Out Successfully";

            return RedirectToAction("index", "login");


        }

    }
}
