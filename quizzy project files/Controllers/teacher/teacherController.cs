using Microsoft.AspNetCore.Mvc;
using Quizzy.Helpers;
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
            return View("mainpage_teacher");
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
            ViewBag.Subjects=dt;    
            return View("selectSub");
        }


        [HttpPost]
        public IActionResult subjectSelect(string selectedSubjectId)
        {
            // Store in session
            HttpContext.Session.SetString("subjectID", selectedSubjectId);
            
            return RedirectToAction("home");
        }


    }

}
