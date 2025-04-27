using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Layer;
using Quizzy.Models.Buisness_Models;

namespace Quizzy.Controllers.login
{
    public class LoginController : Controller
    {
        
        public IActionResult Index()
        {
            return View("login_page");
        }

        public IActionResult register()
        {
            return View("register_page");
        }

        public IActionResult stuReg()
        {
            return View("student_reg");
        }

        public IActionResult teacherReg()
        {
            return View("teacher_reg");
        }


        [HttpPost]
        public IActionResult Index(login_models model)
        {
            Console.WriteLine($"A user with email: {model.email} just tried to login");
            login_page login_Page = new login_page();
            login_Page.login(model);    

            return RedirectToAction("index");

        }
        [HttpPost]

        public IActionResult register(Registraton_models reg)
        {
            Console.WriteLine($"user with email {reg.email} and role {reg.role} is trying to sign up");

            if (reg.role == "student")
            {
               
                return RedirectToAction("stuReg");
            }
            else if (reg.role == "teacher")
            {
                return RedirectToAction("teacherReg");
            }

            return RedirectToAction("index");
        }


        [HttpPost]

        public IActionResult stuReg(Student stu)
        {
            Console.WriteLine($"a new student with email {stu.email} and name as {stu.last_name} and password {stu.password} has sign up on date {stu.addmission_year} and has role {stu.role}");



            signUp_student signUp_Student = new signUp_student();
            signUp_Student.check(stu);
            return RedirectToAction("index");
        }



        [HttpPost]

        public IActionResult teacherReg(Teacher model)
        {
            Console.WriteLine($"name is {model.last_name}");
            return RedirectToAction("index");
        }
    }
}
