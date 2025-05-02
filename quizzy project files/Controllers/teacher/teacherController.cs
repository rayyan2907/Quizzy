using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.student;
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


            if (teacher == null || subject == null)
            {

                TempData["log"] = "Session Expired";

                return RedirectToAction("index", "login");
            }


            DataTable enrolled = teacherBL.statsTotalstu(subject.subjectID);
            string enroll = enrolled.Rows[0]["total_stu"].ToString();

            DataTable compquiz = teacherBL.statsComplete(subject.subjectID);
            string completequiz = compquiz.Rows[0]["quizes"].ToString();

            DataTable upcomming = teacherBL.statsUpcomming(subject.subjectID);
            string upcomm = upcomming.Rows[0]["quizes"].ToString();

            DataTable aggregate = teacherBL.statsAvg(subject.subjectID);

            string agregate;
            if (aggregate == null || aggregate.Rows.Count == 0)
            {
                agregate = "0.00";
            }
            else
            {
                agregate = aggregate.Rows[0]["aggregate"].ToString();
            }
            
            ViewBag.enroll = enroll;
            ViewBag.compquiz = completequiz;
            ViewBag.upcomming = upcomm;
            ViewBag.aggregate = agregate;



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
            if (teacher == null)
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

        public IActionResult studentManage()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject==null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine($"{subject.name}");

            DataTable dt1 = StudentBL.viewReq(subject.subjectID);

            Console.WriteLine(dt1.Rows.Count);
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.student = dt1;
            return View("studentManage");
        }
        [HttpPost]
        public IActionResult assign(string studentID)
        {
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Enrollment e = new Enrollment();
            e.stuID= studentID;
            e.courseID = subject.subjectID;
            e.status = true;

            if (StudentBL.updateAssign(e))
            {
                TempData["Check"] = "Student enrolled in the course successfully";

                string serverAddress = $"{Request.Scheme}://{Request.Host}";
                string loginUrl = $"{serverAddress}/login/index";
                var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
                Student stu = StudentBL.getData(studentID);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(stu.first_name + " " + stu.last_name, stu.email));
                message.Subject = $"{subject.name} ({subject.code}) - Enrollment Request Accepeted";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                               <html>
                                 <body style='font-family: Arial, sans-serif; background-color: white; color: #1B3D1B; padding: 30px;'>
                                     <div style='background-color: #E6F4EA; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                         <h2 style='color: #2E8B57;'>Enrollment Request Approved!</h2>

                                         <p>Dear <h1 style=""margin: 0;"">{stu.first_name} {stu.last_name}</h1></p>

                                         <h2 style='color: #2E8B57;'>Congratulations!</h2>
                                         <p>Your request to enroll in the course <b>{subject.code} - {subject.name}</b> has been <strong>approved</strong> by the course instructor <strong>Prof. {teacher.first_name} {teacher.last_name}</strong>.</p>
                                         <p>Your roll number is <b>{stu.addmission_year}-{stu.dept}-{stu.roll_num}</b>, and you are now officially part of the <b>{stu.dept}</b> department.</p>

                                         <p>You can now access course materials, assignments, and updates through your Quizzy dashboard.</p>

                                         <div style='text-align: center; margin-top: 20px;'>
                                             <a href='{loginUrl}' style='background-color: #2E8B57; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Go to Dashboard</a>
                                         </div>

                                         <div style='margin-top: 40px; font-size: 12px; color: #777; text-align: center;'>
                                             <footer style='font-size: 12px; color: #555;'>Happy Learning!</footer>
                                             &copy; 2025 Quizzy. All rights reserved.
                                         </div>
                                     </div>
                                 </body>
                                </html>

                               
                                    "

                };
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    TempData["log"] = "Internet not connected";
                    Console.WriteLine("internet issue");

                }



            }
            else
            {
                TempData["log"] = "Error in enrolling student";
            }

            return RedirectToAction("studentManage");


        }

        public IActionResult unassign(string studentID)
        {
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Enrollment e = new Enrollment();
            e.stuID = studentID;
            e.courseID = subject.subjectID;
            e.status = false;

            if (StudentBL.updateAssign(e))
            {
                TempData["Check"] = "Student unenrolled in the from course successfully";



                string serverAddress = $"{Request.Scheme}://{Request.Host}";
                string loginUrl = $"{serverAddress}/login/index";
                var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
                Student stu = StudentBL.getData(studentID);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(stu.first_name + " " + stu.last_name, stu.email));
                message.Subject = $"{subject.name} ({subject.code}) - Enrollment Cancelled";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                               <html>
                                    <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                                    <div style='background-color: #FFF5F5; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                        <h2 style='color: #FF5C5C;'>Enrollment Cancelled</h2>

                                        <p>Dear <h1>{stu.first_name} {stu.last_name}</h1></p>

                                        <p>We regret to inform you that your enrollment in the course <b>{subject.code} - {subject.name}</b> has been <strong>revoked</strong> by the course instructor <strong>Prof. {teacher.first_name} {teacher.last_name}</strong>.</p>
        
                                        <p>Your roll number <b>{stu.addmission_year}-{stu.dept}-{stu.roll_num}</b> is no longer associated with this course under the <b>{stu.dept}</b> department.</p>

                                        <p>If you believe this was a mistake or you would like to re-enroll, please contact your course instructor or request enrollment again from your dashboard.</p>

                                        <div style='text-align: center; margin-top: 20px;'>
                                            <a href='{loginUrl}' style='background-color: #FF5C5C; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Visit Dashboard</a>
                                        </div>

                                        <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                            <footer style='font-size: 12px; color: #888;'>We're here to help if you need any assistance.</footer>
                                            &copy; 2025 Quizzy. All rights reserved.
                                        </div>
                                    </div>
                                    </body>
                                    </html>

                                    "

                };
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    TempData["log"] = "Internet not connected";
                    Console.WriteLine("internet issue");

                }
            }
            else
            {
                TempData["log"] = "Error in unenrolling student";
            }

            return RedirectToAction("studentManage");


        }



        public IActionResult announcements()
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


            return View("announcements");
        }


        [HttpPost]

        public IActionResult sendAnnouncement(Announcements a)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            DataTable dt = teacherBL.annnounce(subject.subjectID);
            Console.WriteLine(dt.Rows.Count);
            if (dt == null || dt.Rows.Count==0)
            {
                TempData["log"] = "You do not have any enrolled students yet";
                return RedirectToAction("home");
            }

            string serverAddress = $"{Request.Scheme}://{Request.Host}";
            string loginUrl = $"{serverAddress}/login/index";

            Console.WriteLine($"an announcement \"{a.body}\" was made by teacher {teacher.first_name} {teacher.last_name} ");

            foreach (DataRow row in dt.Rows)
            {
                string stu_first = row["first_name"].ToString();
                string stu_last = row["last_name"].ToString();
                string stu_email = row["email"].ToString();


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress( stu_first+ " " + stu_last, stu_email));
                message.Subject = $"Annoucement ({subject.code}) - {a.subject}";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                              <html>
                                    <body style=""font-family: Arial, sans-serif; background-color: #ffffff; color: #2E2B41; padding: 30px;"">
                                        <div style=""background-color: #E8F5E9; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                                            <h2 style=""color: #388E3C;"">📢 New Announcement</h2>

                                            <p>Dear, <h1>{stu_first} {stu_last}</h1></p>

                                            <p>You have received a new announcement from <strong>Prof. {teacher.first_name} {teacher.last_name}</strong> for the course <strong>{subject.code} - {subject.name}</strong>.</p>

                                            <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"" />

                                            <h3 style=""color: #2E7D32;"">{a.subject}</h3>
                                            <p style=""line-height: 1.6;"">{a.body}</p>
                                            

                                            <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"" />
                                            <p>Best Regards,
                                            {teacher.first_name} {teacher.last_name}</p>
                                            <p>Stay updated by checking your Quizzy dashboard regularly.</p>
                                            
                                            

                                            <div style=""text-align: center; margin-top: 20px;"">
                                                <a href=""{loginUrl}"" style=""background-color: #388E3C; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;"">Go to Dashboard</a>
                                            </div>

                                            <div style=""margin-top: 40px; font-size: 12px; color: #999; text-align: center;"">
                                                <footer style=""font-size: 12px; color: #888;"">Keep Learning with Quizzy</footer>
                                                &copy; 2025 Quizzy. All rights reserved.
                                            </div>
                                        </div>
                                    </body>
                                    </html>

                                    "

                };
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    TempData["log"] = "Internet not connected";
                    Console.WriteLine("internet issue");

                }
                Console.WriteLine($"Email sent to {stu_first} {stu_last} at email {stu_email}");
            }

            TempData["Check"] = "Email sent to all the enrolled students";
            return RedirectToAction("home");
        }
    }
}
