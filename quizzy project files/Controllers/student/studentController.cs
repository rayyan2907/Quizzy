using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Mysqlx.Crud;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Models;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Quizzy.Controllers.student
{
    public class studentController : Controller
    {
        Student stu = new Student();


       

        public IActionResult availableQuizes()
        {
            return View("seeOpenQuiz");
        }
        public IActionResult main()
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
            return RedirectToAction("enrolledCourse");
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

        [HttpGet]

        public IActionResult enrolledCourse()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");

            if (stu == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }
            DataTable dt = StudentBL.getStuCourses(stu.stuID);

            ViewBag.Enrolled = dt;
            ViewBag.stu = stu;

            return View("mainPage_student");

        }

        [HttpGet]
        public IActionResult availableCourse()
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");
            DataTable dt = StudentBL.getAllStuCourses();

            ViewBag.Courses = dt;
            ViewBag.stu = stu;

            return View("viewAllCourses");
        }

        [HttpPost]

        public IActionResult Enroll(string courseId)
        {
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");
            Enrollment e = new Enrollment();
            e.stuID = stu.stuID;
            e.courseID = courseId;
            e.status = false;
            if (StudentBL.CheckIfEnrolled(e))
            {


                bool flag = StudentBL.stuEnroll(e);

                if (!flag)
                {
                    TempData["Check"] = "Error in enrollment";
                }
                else
                {
                    TempData["log"] = "Enrollment pending!";

                    Teacher teacher = new Teacher();
                    subject_model sub = new subject_model();
                    sub = StudentBL.getSub(courseId);
                    teacher = StudentBL.getTec(sub.teacherID);
                    string serverAddress = $"{Request.Scheme}://{Request.Host}";
                    string loginUrl = $"{serverAddress}/login/index";

                    Console.WriteLine($"studemt with name {stu.first_name} {stu.last_name} has enrolled in the course {sub.code}-{sub.name} which is taught by {teacher.first_name} {teacher.last_name}");

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message.To.Add(new MailboxAddress(stu.first_name + " " + stu.last_name, stu.email));
                    message.Subject = "Subject Enrollment Pending - Quizzy";

                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                                <html>
                                <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                                    <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                        <h2 style='color: #8672FF;'>Request Sent Successfully!</h2>

                                        <p>Dear,<h1> {stu.first_name} {stu.last_name}</h1></p>

                                        <h2 style='color: #8672FF;'>Great News!</h2>
                                        <p>Your request to enroll in the course <b>{sub.code}-{sub.name}</b> has been sent to the course instructor <strong>Proff. {teacher.first_name} {teacher.last_name}</strong>.</p>
                                        <p>Your roll number is <b>{stu.addmission_year}-{stu.dept}-{stu.roll_num}</b> and you have been registered in the <b>{stu.dept}</b> Department.</p>
                                        <p>Please note that your request is currently under review by the instructor. You will receive a notification once your enrollment is accepted.</p>
                                        <p>In the meantime, feel free to explore other courses or visit your dashboard for updates.</p>
        
        
                                        <div style='text-align: center; margin-top: 20px;'>
                                            <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Login to Quizzy</a>
                                        </div>

                                        <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                            <footer style='font-size: 12px; color: #888;'>Happy Learning!</footer>
                                            &copy; 2025 Quizzy. All rights reserved.
                                        </div>
                                    </div>
                                </body>
                                </html>"

                    };
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                        client.Send(message);
                        client.Disconnect(true);
                    }
                    
                    //to the teacher

                    var message1= new MimeMessage();
                    message1.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message1.To.Add(new MailboxAddress(teacher.first_name + " " + teacher.last_name, teacher.email));
                    message1.Subject = "New Enrollment Request - Quizzy";

                    message1.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                        <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                            <h2 style='color: #8672FF;'>New Enrollment Request</h2>

                            <p>Dear, <h1>Prof. {teacher.first_name} {teacher.last_name}</h1></p>

                            <p>We are pleased to inform you that a student has requested to enroll in your course <b>{sub.code}-{sub.name}</b>.</p>
                            <p><b>{stu.first_name} {stu.last_name}</b> (Roll Number: <b>{stu.addmission_year}-{stu.dept}-{stu.roll_num}</b>) has submitted a request to join the course. The student is from the <b>{stu.dept}</b> Department.</p>
                            <p>Please review the request and accept or reject it accordingly. You can do so from your instructor dashboard.</p>

                            <div style='text-align: center; margin-top: 20px;'>
                                <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Instructor Dashboard</a>
                            </div>

                            <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                <footer style='font-size: 12px; color: #888;'>Thank you for your attention!</footer>
                                &copy; 2025 Quizzy. All rights reserved.
                            </div>
                        </div>
                        </body>
                        </html>"


                    };
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                        client.Send(message1);
                        client.Disconnect(true);
                    }


                }
            }
            else
            {
                TempData["Check"] = "You have already enrolled in this course";
                return RedirectToAction("availableCourse");

            }
            return RedirectToAction("enrolledCourse");
        }

       

    }
}
