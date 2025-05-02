using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Layer.teacher;
using Quizzy.Models.Buisness_Models;

namespace Quizzy.Controllers.subject
{
    public class subjectController : Controller
    {
        subjectBL subject = new subjectBL();
        public IActionResult addSubject()
        {
            return View("~/Views/teacher/addNewSubject.cshtml");
        }


        [HttpPost]

        public IActionResult addSubject(subject_model sub)
        {

            if (HttpContext.Session.GetString("teacId") == null)
            {
                TempData["log"] = "Session not found";

                return RedirectToAction("index", "login");
            }

            sub.teacherID = HttpContext.Session.GetString("teacId");

            string msg = subject.subjectAdd(sub);

            if (msg == "Error in adding subject" || msg == "Error in adding subject. Conversion not possible")
            {
                TempData["log"] = msg;
            }
            else
            {
                TempData["Check"] = msg;


                Teacher t = new Teacher();
                t = teacherBL.getData(sub.teacherID);
                string serverAddress = $"{Request.Scheme}://{Request.Host}";
                string loginUrl = $"{serverAddress}/login/index";
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(t.first_name + " " + t.last_name, t.email));
                message.Subject = "Subject Added Successfully - Quizzy";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"                    
                        <html>
                        <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                            <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                <h2 style='color: #8672FF;'>Subject Added Successfully!</h2>

                                <p>Dear,<h1> {t.first_name} {t.last_name}</h1></p>

                                <p>You have successfully added a new subject <strong>{sub.name}</strong> (Code: {sub.code}) to the <strong>Quizzy</strong> platform.</p>
                                <p>We are excited to have your new course available for students!</p>

                                <p>Thank you for your dedication to education.</p>
        
                                <div style='text-align: center; margin-top: 20px;'>
                                    <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Manage Your Subjects</a>
                                </div>

                                <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                    <footer style='font-size: 12px; color: #888;'>Keep up the great work!</footer>
                                    &copy; 2025 Quizzy. All rights reserved.
                                </div>
                            </div>
                        </body>
                        </html>"

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
            }

            return RedirectToAction("subjectSelect","teacher");
        }
    }
    
}
