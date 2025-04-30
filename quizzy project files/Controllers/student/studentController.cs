using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Controllers.student
{
    public class studentController : Controller
    {
        Student stu = new Student();
        public IActionResult mainPage()
        {
            string id = HttpContext.Session.GetString("UserId");
            Console.WriteLine(id);

            if (string.IsNullOrEmpty(id))
                {
                    // You can reload from cookie or redirect to login
                    if (Request.Cookies["UserId"] != null)
                    {
                        id = Request.Cookies["UserId"];
                        HttpContext.Session.SetString("UserId", id); // restore session
                        Console.WriteLine($"an old session of student with id {id} was found ");
                    }
                    else
                    {
                        TempData["log"] = "Session not found";

                        return RedirectToAction("index", "login");
                    }
                }
           
            stu = StudentBL.getData(id);
            HttpContext.Session.SetObject("StudentObj", stu);

            return View("mainPage_student",stu);
        }

        public IActionResult logOut()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");

            Console.WriteLine($"user with name {stu.first_name} {stu.last_name} is loging out");
            HttpContext.Session.Clear();

            stu = null;
            if (Request.Cookies["UserId"] != null)
            {
                Response.Cookies.Delete("UserId");
            }

            TempData["check"] = "Logged Out Successfully";

            return RedirectToAction("index", "login");
        }
       
       
    }
}
