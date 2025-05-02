using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Quizzy.Helpers;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Models.Buisness_Layer.teacher;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.quiz;
using System.Data;
using System.Globalization;

namespace Quizzy.Controllers.createQuiz
{
    public class createQuizController : Controller
    {
        public IActionResult quizMake()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }


            DataTable dt = getQuizBL.getQuiz(subject.subjectID);

            ViewBag.quiz = dt;
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            Console.WriteLine($"{subject.name}");

            return View("showQuiz");
        }

        
        [HttpPost]

        public IActionResult updatequiz(string id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            Console.WriteLine(id);

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            
            return RedirectToAction("quizMake");                
        }

        [HttpPost]   
        
        public IActionResult deletequiz(string id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(id);
            bool deleted = createQuizBL.deleteQuiz(id);
            if(deleted)
            {
                TempData["Check"] = "Quiz deleted successfully.";
            }
            else
            {
                TempData["log"] = "Cannot delete Quiz";
            }
            
            return RedirectToAction("quizMake");
        }

        public IActionResult assignQuiz(string id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine($"the quiz with id {id} is un assigned");

            bool assigned = createQuizBL.assignQuiz(id);
            if (assigned)
            {
                TempData["Check"] = "Quiz assigned successfully.";



                DataTable dt = teacherBL.annnounce(subject.subjectID);
                Console.WriteLine(dt.Rows.Count);
                if (dt == null || dt.Rows.Count == 0)
                {
                    TempData["log"] = "You do not have any enrolled students yet";
                    return RedirectToAction("quizMake");
                }

                quiz_model q = getQuizDL.getQuizData(id);

                string serverAddress = $"{Request.Scheme}://{Request.Host}";
                string loginUrl = $"{serverAddress}/login/index";

                Console.WriteLine($"the quiz annoucement for {q.quizName} is being sent to all reg maills ");

                foreach (DataRow row in dt.Rows)
                {
                    string stu_first = row["first_name"].ToString();
                    string stu_last = row["last_name"].ToString();
                    string stu_email = row["email"].ToString();


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message.To.Add(new MailboxAddress(stu_first + " " + stu_last, stu_email));
                    message.Subject = $"Annoucement ({subject.code}) - Quiz Assigned";

                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                                    <html>
                                    <body style=""font-family: Arial, sans-serif; background-color: #ffffff; color: #2E2B41; padding: 30px;"">
                                        <div style=""background-color: #FFF3E0; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                                            <h2 style=""color: #EF6C00;"">📝 New Quiz Assigned</h2>

                                            <p>Dear <strong>{stu_first} {stu_last}</strong>,</p>

                                            <p>You have been assigned a new quiz {q.quizName} by <strong>Prof. {teacher.first_name} {teacher.last_name}</strong> for the course <strong>{subject.code} - {subject.name}</strong>.</p>

                                            <p style=""color: #D84315; font-weight: bold;"">Please complete this quiz as soon as possible. You will have only {q.given_time} mins to solve once you start the quiz </p>


                                          
                                            <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"" />
                                            <p>Best regards,</p>
                                            <p><strong>{teacher.first_name} {teacher.last_name}</strong></p>
                                            <p>Stay on top of your tasks via the Quizzy dashboard.</p>

                                            <div style=""text-align: center; margin-top: 20px;"">
                                                <a href=""{loginUrl}"" style=""background-color: #EF6C00; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;"">Go to Quiz Now</a>
                                            </div>

                                            <div style=""margin-top: 40px; font-size: 12px; color: #999; text-align: center;"">
                                                <footer style=""font-size: 12px; color: #888;"">Empowering learning through Quizzy</footer>
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
                return RedirectToAction("quizMake");
            }
        
            else
            {
                TempData["log"] = "Cannot assign Quiz";
            }

            return RedirectToAction("quizMake");
        }

        public IActionResult unassignQuiz(string id)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            Console.WriteLine($"the quiz with id {id} is un assigned");


            bool assigned = createQuizBL.unassignQuiz(id);
            if (assigned)
            {
                TempData["Check"] = "Quiz unassigned successfully.";



                DataTable dt = teacherBL.annnounce(subject.subjectID);
                Console.WriteLine(dt.Rows.Count);
                if (dt == null || dt.Rows.Count == 0)
                {
                    TempData["log"] = "You do not have any enrolled students yet";
                    return RedirectToAction("quizMake");
                }

                quiz_model q = getQuizDL.getQuizData(id);

                string serverAddress = $"{Request.Scheme}://{Request.Host}";
                string loginUrl = $"{serverAddress}/login/index";

                Console.WriteLine($"the quiz annoucement for {q.quizName} is being sent to all reg maills ");

                foreach (DataRow row in dt.Rows)
                {
                    string stu_first = row["first_name"].ToString();
                    string stu_last = row["last_name"].ToString();
                    string stu_email = row["email"].ToString();


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message.To.Add(new MailboxAddress(stu_first + " " + stu_last, stu_email));
                    message.Subject = $"Annoucement ({subject.code}) - Quiz UnAssigned";

                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                                  
                                            <html>
                                                <body style=""font-family: Arial, sans-serif; background-color: #ffffff; color: #2E2B41; padding: 30px;"">
                                                    <div style=""background-color: #FFEBEE; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                                                        <h2 style=""color: #C62828;"">⚠️ Quiz Unassigned</h2>

                                                        <p>Dear <strong>{stu_first} {stu_last}</strong>,</p>

                                                        <p>We would like to inform you that the quiz titled <strong>{q.quizName}</strong> has been unassigned by <strong>Prof. {teacher.first_name} {teacher.last_name}</strong> for the course <strong>{subject.code} - {subject.name}</strong>.</p>

                                                        <p style=""color: #B71C1C; font-weight: bold;"">You will no longer be able to attempt this quiz. If you have already attempted the quiz, then there is no need to worry</p>

                                                        <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;""/>
                                                        <p>If you have any questions or concerns, please reach out to your teacher directly.</p>

                                                        <p>Stay updated by checking your Quizzy dashboard regularly.</p>

                                                        <div style=""text-align: center; margin-top: 20px;"">
                                                            <a href=""{loginUrl}"" style=""background-color: #C62828; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;"">Go to Dashboard</a>
                                                        </div>

                                                        <div style=""margin-top: 40px; font-size: 12px; color: #999; text-align: center;"">
                                                            <footer style=""font-size: 12px; color: #888;"">Quizzy — Manage your learning effectively</footer>
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
                return RedirectToAction("quizMake");
            
             }
            else
            {
                TempData["log"] = "Cannot unassign Quiz";
            }

            return RedirectToAction("quizMake");
        }

        public IActionResult newQuiz()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }


            DataTable dt = getQuizBL.getQuiz(subject.subjectID);

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            return View("newQuiz");
        }

        [HttpPost]

        public IActionResult newQuiz(quiz_model quiz)
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            Console.WriteLine(quiz.quizName);

            quiz.subID = subject.subjectID;
            


            ViewBag.subject = subject;
            ViewBag.teacher = teacher;



            string serverAddress = $"{Request.Scheme}://{Request.Host}";
            string loginUrl = $"{serverAddress}/login/index";
            string msg = createQuizBL.addQuiz(quiz);
            if (msg== "The quiz has been created successfully")
            {
                TempData["Check"] = msg;



                Console.WriteLine(msg);


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(teacher.first_name + " " + teacher.last_name, teacher.email));
                message.Subject = $"({subject.code}) Quiz Creation Alert - Quizzy";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                                                    <!DOCTYPE html>
                                <html>
                                  <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f6fb; padding: 20px; color: #333;"">
                                    <div style=""max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); padding: 30px;"">
      
                                      <h2 style=""text-align: center; color: #5a8dee; margin-bottom: 20px;"">📝 Quiz Created Successfully</h2>
      
                                      <p>Dear, <h2>{teacher.first_name} {teacher.last_name}</h2></p>
      
                                      <p>Your new quiz ""<strong>{quiz.quizName}</strong>"" has been successfully created in the course <strong>{subject.code} - {subject.name}</strong>.</p>

                                      <div style=""background-color: #f1f5ff; padding: 15px; border-radius: 8px; margin: 20px 0;"">
                                        <p><strong>Quiz Name:</strong> {quiz.quizName}</p>
                                        <p><strong>Allowed Time:</strong> {quiz.given_time} minutes</p>
                                       
                                      </div>

                                      <div style=""text-align: center;"">
                                        <a href=""{loginUrl}"" style=""display: inline-block; margin-top: 20px; padding: 12px 24px; background-color: #5a8dee; color: white; text-decoration: none; border-radius: 8px; font-weight: bold;"">View Quiz</a>
                                      </div>

                                      <p style=""margin-top: 30px;"">Thank you for using <strong>Quizzy</strong> to manage your assessments.</p>

                                      <div style=""text-align: center; font-size: 14px; color: #777; margin-top: 30px;"">
                                        — Quizzy Team<br>
                                        Need help? <a href=""mrayyan403@gmail.com"" style=""color: #5a8dee;"">Contact Support</a>
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
                    TempData["Check"] = "Internet not connected";
                    Console.WriteLine("internet issue");

                }

                Console.WriteLine("email sent to teavher");

            }
            else
            {
                TempData["log"] = msg;
            }
            Console.WriteLine("the status of quiz is " + quiz.isAssign);
            if (quiz.isAssign == true)
            {
                DataTable dt = teacherBL.annnounce(subject.subjectID);
                Console.WriteLine(dt.Rows.Count);
                if (dt == null || dt.Rows.Count == 0)
                {
                    TempData["log"] = "As you do not have any enrolled students so mail has not been sent to any one";
                    return RedirectToAction("quizMake");
                }




                Console.WriteLine($"the quiz annoucement for {quiz.quizName} is being sent to all reg maills ");

                foreach (DataRow row in dt.Rows)
                {
                    string stu_first = row["first_name"].ToString();
                    string stu_last = row["last_name"].ToString();
                    string stu_email = row["email"].ToString();


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message.To.Add(new MailboxAddress(stu_first + " " + stu_last, stu_email));
                    message.Subject = $"Annoucement ({subject.code}) - Quiz Assigned";

                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                                    <html>
                                    <body style=""font-family: Arial, sans-serif; background-color: #ffffff; color: #2E2B41; padding: 30px;"">
                                        <div style=""background-color: #FFF3E0; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                                            <h2 style=""color: #EF6C00;"">📝 New Quiz Assigned</h2>

                                            <p>Dear <strong>{stu_first} {stu_last}</strong>,</p>

                                            <p>You have been assigned a new quiz {quiz.quizName} by <strong>Prof. {teacher.first_name} {teacher.last_name}</strong> for the course <strong>{subject.code} - {subject.name}</strong>.</p>

                                            <p style=""color: #D84315; font-weight: bold;"">Please complete this quiz as soon as possible. You will have only {quiz.given_time} mins to solve once you start the quiz </p>


                                          
                                            <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"" />
                                            <p>Best regards,</p>
                                            <p><strong>{teacher.first_name} {teacher.last_name}</strong></p>
                                            <p>Stay on top of your tasks via the Quizzy dashboard.</p>

                                            <div style=""text-align: center; margin-top: 20px;"">
                                                <a href=""{loginUrl}"" style=""background-color: #EF6C00; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;"">Go to Quiz Now</a>
                                            </div>

                                            <div style=""margin-top: 40px; font-size: 12px; color: #999; text-align: center;"">
                                                <footer style=""font-size: 12px; color: #888;"">Empowering learning through Quizzy</footer>
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
                return RedirectToAction("quizMake");

            }


            return RedirectToAction("quizMake");
        }
    }
}
