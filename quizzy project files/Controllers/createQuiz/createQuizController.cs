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
            var quiz = HttpContext.Session.GetObject<quiz_model>("quizObj");
            if (quiz != null)
            {
                quiz = null;
                HttpContext.Session.Remove("quizObj");
                Console.WriteLine("quiz obj haas been removed");
            }
            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            if (HttpContext.Session.GetString("quizID") != null)
            {
                Console.WriteLine($"we have cleared the quiz id from the session");
                HttpContext.Session.Remove("quizID");
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
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            quiz_model quiz = getQuizBL.getQuizdata(id);
            Console.WriteLine(" the quiz tobe updsaatd is " + id);

            HttpContext.Session.SetString("quizID", id);

            Console.WriteLine("quiz id in post is " + id);
            return View("updateQuiz",quiz);                
        }

        

        public IActionResult updatequiz_2()
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            string id;
            if (HttpContext.Session.GetString("quizID") != null)
            {
                id = HttpContext.Session.GetString("quizID");
                Console.WriteLine(id);
                Console.WriteLine("quiz id in get is " + id);
            }
            else
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            quiz_model quiz = createQuizBL.getQuizObj(id);
            Console.WriteLine(" the quiz is updated" + id);



            return View("updateQuiz", quiz);
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
                    message.Subject = $"{subject.code}-{subject.name}  - Quiz Assigned";

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
                    message.Subject = $"{subject.code}-{subject.name}  - Quiz UnAssigned";

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
            quiz.isAssign = false;


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
                    message.Subject = $"{subject.code}-{subject.name} - Quiz Assigned";

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


        [HttpPost]

        public IActionResult updatequizdata(quiz_model quiz)
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            if (HttpContext.Session.GetString != null)
            {
                quiz.quizID = HttpContext.Session.GetString("quizID");
                Console.WriteLine(" the quiz tobe updsaatd is "+ quiz.quizID);
            }
            else
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");

            }
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
            string msg = createQuizBL.updateQuiz(quiz);
            if (msg == "The quiz has been updated successfully")
            {
                TempData["Check"] = msg;



                Console.WriteLine(msg);


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(teacher.first_name + " " + teacher.last_name, teacher.email));
                message.Subject = $"({subject.code}) Quiz Update Alert - Quizzy";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                                                    <!DOCTYPE html>
                                <html>
                                  <body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f6fb; padding: 20px; color: #333;"">
                                    <div style=""max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); padding: 30px;"">
      
                                      <h2 style=""text-align: center; color: #5a8dee; margin-bottom: 20px;"">📝 Quiz Updated Successfully</h2>
      
                                      <p>Dear, <h2>{teacher.first_name} {teacher.last_name}</h2></p>
      
                                      <p>Details of quiz ""<strong>{quiz.quizName}</strong>"" has been successfully updated in the course <strong>{subject.code} - {subject.name}</strong>.</p>

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
                    Console.WriteLine("email sent to teacher");
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
            if (quiz.isAssign == true && msg== "The quiz has been updated successfully")
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
                    message.Subject = $"{subject.code}-{subject.name} - Quiz Assigned";

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


       

        [HttpPost]
        public IActionResult showMcq(string id)
        {
            Console.WriteLine("id at start is " + id);
            Console.WriteLine("page 2");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
           

            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);

            HttpContext.Session.SetObject("quizObj", q);


            Console.WriteLine("id in controller is " + id);

            Console.WriteLine($"quiz id is {q.quizID} and {q.subID}");
            DataTable dt = createQuizBL.getMcqs(q);
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.mcq = dt;
            ViewBag.id = id;


            return View("mcqTable");
        }

        [HttpGet]
        public IActionResult showMcq_2(string id)
        {
            Console.WriteLine("id at start is " + id);
            Console.WriteLine("page 2");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
           
            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);

            HttpContext.Session.SetObject("quizObj", q);


            Console.WriteLine("id in controller is " + id);

            Console.WriteLine($"quiz id is {q.quizID} and {q.subID}");
            DataTable dt = createQuizBL.getMcqs(q);
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.mcq = dt;
            ViewBag.id = id;


            return View("mcqTable");

        }

        public IActionResult createMcq()
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
            return View("createMcq");

        }

        [HttpPost]

        public IActionResult createMcq(mcq_model m)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            var quiz = HttpContext.Session.GetObject<quiz_model>("quizObj");


            if (teacher == null || subject == null || quiz==null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            Console.WriteLine("quiz obj made");

            m.quizID = quiz.quizID;

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            bool flag = createQuizBL.addMcq(m);

            if (flag)
            {
                TempData["Check"] = "Mcq Added Succesfully";
            }
            else
            {
                TempData["log"] = "Error in adding";
            }
            HttpContext.Session.Remove("quizObj");
            quiz = null;
            Console.WriteLine("quiz obj destroyed");

            return RedirectToAction("showMcq_2", new { id = m.quizID });



        }


        [HttpPost]
        public IActionResult showShqs(string id)
        {
            Console.WriteLine("id at start of ans is " + id);
            Console.WriteLine("page ans 1");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
           
            

            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);

            HttpContext.Session.SetObject("quizObj", q);


            Console.WriteLine("id in controller is " + id);

            Console.WriteLine($"quiz id is {q.quizID} and {q.subID}");
            DataTable dt = createQuizBL.getShqs(q);
            Console.WriteLine("Rows in shq DataTable: " + dt.Rows.Count);
           

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.shq = dt;
            ViewBag.id = id;


            return View("shqTable");
        }

        [HttpGet]
        public IActionResult showShqs_2(string id)
        {
            Console.WriteLine("id at start of ans  is " + id);
            Console.WriteLine("page ans 2");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            

            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);

            HttpContext.Session.SetObject("quizObj", q);


            Console.WriteLine("id in controller is " + id);

            Console.WriteLine($"quiz id is {q.quizID} and {q.subID}");
            DataTable dt = createQuizBL.getShqs(q);
            Console.WriteLine("Rows in shq DataTable: " + dt.Rows.Count);
           

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.shq = dt;
            ViewBag.id = id;


            return View("shqTable");
        }

        public IActionResult createShq()
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
            return View("createShq");

        }
        [HttpPost]
        public IActionResult createShq(shq_model s)
        {
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");
            var quiz = HttpContext.Session.GetObject<quiz_model>("quizObj");


            if (teacher == null || subject == null || quiz==null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            Console.WriteLine("quiz obj made");

            s.quizID = quiz.quizID;

            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            bool flag = createQuizBL.addShq(s);

            if (flag)
            {
                TempData["Check"] = "Mcq Added Succesfully";
            }
            else
            {
                TempData["log"] = "Error in adding";
            }
            HttpContext.Session.Remove("quizObj");
            quiz = null;
            Console.WriteLine("quiz obj destroyed");
            return RedirectToAction("showShqs_2", new { id = s.quizID });



        }

        
        public IActionResult showMcqForUpdate()
        {
            string id;
            if (HttpContext.Session.GetString("quizID") != null)
            {
                id = HttpContext.Session.GetString("quizID");
                Console.WriteLine(id);
                Console.WriteLine("quiz id in get is " + id);
            }
            else
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            Console.WriteLine("id at start is " + id);
            Console.WriteLine("page 2");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }


            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);

           

            Console.WriteLine($"quiz id is {q.quizID} and {q.subID} for update ");
            DataTable dt = createQuizBL.getMcqs(q);
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.mcq = dt;
            ViewBag.id = id;


            return View("updateMcqTable");
        }
        public IActionResult shoqShqForUpdate()
        {
            string id;
            if (HttpContext.Session.GetString("quizID") != null)
            {
                id = HttpContext.Session.GetString("quizID");
                Console.WriteLine(id);
                Console.WriteLine("quiz id in get is " + id);
            }
            else
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            Console.WriteLine("id at start is " + id);
            Console.WriteLine("page 2");
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }


            quiz_model q = new quiz_model();

            q = createQuizBL.getQuizObj(id);


            Console.WriteLine($"quiz id is {q.quizID} and {q.subID}");
            DataTable dt = createQuizBL.getShqs(q);
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;
            ViewBag.shq = dt;
            ViewBag.id = id;


            return View("updateShortTable");
        }

        [HttpGet]
        public IActionResult deleteMcq(string id)
        {
            Console.WriteLine("id of mcq id is " + id);
            bool flag = createQuizBL.mcqDel(id);
            if (flag)
            {
                TempData["Check"] = "MCQ deleted successfully";
                Console.WriteLine("mcq deleted successfully");

            }
            else
            {
                TempData["log"] = "Error in deleting mcq";
            }
            return RedirectToAction("showMcqForUpdate");
        }


        [HttpGet]
        public IActionResult deleteShq(string id)
        {
            Console.WriteLine("id of shq id is " + id);
            bool flag = createQuizBL.shqDel(id);
            if (flag)
            {
                TempData["Check"] = "SHQ deleted successfully";
                Console.WriteLine("SHQ deleted successfully");

            }
            else
            {
                TempData["log"] = "Error in deleting SHQ";
            }
            return RedirectToAction("shoqShqForUpdate");
        }


        [HttpGet]
        public IActionResult updateMcq(string id)
        {
            
            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            Console.WriteLine(id);

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            mcq_model m = new mcq_model();
            m = createQuizBL.getMCqobj(id);
            Console.WriteLine("mcq id in presentation 1 to be update is " + m.mcq_id);

            return View("updateMcq", m);
        }


        [HttpPost]
        public IActionResult updateMcqData(mcq_model m)
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

            Console.WriteLine("mcq id in presentation to be update is " + m.mcq_id);


            bool flag = createQuizBL.updateMcq(m);
            if (flag)
            {
                TempData["Check"] = "MCQ updated successfully";
                Console.WriteLine("MCQ updated successfully");

            }
            else
            {
                TempData["log"] = "Error in updating MCQ";
            }



            return RedirectToAction("showMcqForUpdate");
        }





        [HttpGet]
        public IActionResult updateShq(string id)
        {

            var teacher = HttpContext.Session.GetObject<Teacher>("teacherObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subjectObj");

            Console.WriteLine(id);

            if (teacher == null || subject == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }
            ViewBag.subject = subject;
            ViewBag.teacher = teacher;

            shq_model s = createQuizBL.getshqObj(id);
            Console.WriteLine("shq id in main one func is "+s.shqID);
            HttpContext.Session.SetString("shqID", s.shqID);
            return View("updateShq", s);
        }


        [HttpPost]
        public IActionResult updateShqData(shq_model s)
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

            s.shqID = HttpContext.Session.GetString("shqID");
            HttpContext.Session.Remove("shqID");

            Console.WriteLine("shq id in main is "+s.shqID);


            bool flag = createQuizBL.updateSHQ(s);
            if (flag)
            {
                TempData["Check"] = "SHQ updated successfully";
                Console.WriteLine("SHQ updated successfully");

            }
            else
            {
                TempData["log"] = "Error in updating SHQ";
            }



            return RedirectToAction("shoqShqForUpdate");
        }

    }
}
