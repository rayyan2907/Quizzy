using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Ocsp;
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
        public IActionResult EnterOtp()
        {
            return View("EnterOtp");
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


            if (reg.password == reg.cnfrm_pwd)
            {


                string generatedOtp = new Random().Next(100000, 999999).ToString();

                // Save OTP in session (or a static/global variable temporarily)
                HttpContext.Session.SetString("EmailOTP", generatedOtp);

                HttpContext.Session.SetString("UserRole", reg.role);
                HttpContext.Session.SetString("UserEmail", reg.email);
                HttpContext.Session.SetString("UserPassword", reg.password);


                Console.WriteLine($"Otp {generatedOtp} has been sent to email {reg.email}");
                // Send email
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(reg.role, reg.email));
                message.Subject = "Your OTP Verification Code";
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                        <html>
                            <body style='font-family: Arial, sans-serif; text-align:center; padding:20px; background-color: #f7f7f7;'>
                                <h2 style='color: #007bff;'>Verify Your Email</h2>
                                <p style='font-size: 16px; color: #333;'>Use the following OTP to verify your account:</p>
                                <div style='font-size:24px; font-weight:bold; margin:20px auto; display:inline-block; padding:10px 20px; border:2px dashed #007bff;'>
                                    {generatedOtp}
                                </div>
                                <p style='font-size: 14px; color: #555;'>This OTP will expire soon. Please verify quickly.</p>
                                <footer style='margin-top: 30px; font-size: 12px; color: #888;'>If you did not request this verification, please ignore this email.</footer>
                            </body>
                        </html>"

                };

                // (Then use SMTP to send the message)
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // NOT your email password! Use Gmail App Password
                    client.Send(message);
                    client.Disconnect(true);
                }



                return RedirectToAction("EnterOtp");
            }
            else
            {
                TempData["ErrorMessage"] = "Passwords does not match!";
                return RedirectToAction("register");

            }
        }


            [HttpPost]

        public IActionResult stuReg(Student stu)
        {
            string serverAddress = $"{Request.Scheme}://{Request.Host}";
            string loginUrl = $"{serverAddress}/login/index";
            string role = HttpContext.Session.GetString("UserRole");
            string email = HttpContext.Session.GetString("UserEmail");
            string password = HttpContext.Session.GetString("UserPassword");

            stu.email = email;
            stu.role = role;
            stu.password = password;


            HttpContext.Session.Remove("UserRole");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserPassword");
            Console.WriteLine($"a new student with email {stu.email} and name as {stu.last_name} has sign up on date {stu.addmission_year} and has role {stu.role}");




            // Send confirmation email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
            message.To.Add(new MailboxAddress(stu.first_name + " " + stu.last_name, stu.email));
            message.Subject = "Registration Successful - Quizzy";
            
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                    <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                        <p>Dear,<h1> {stu.first_name} {stu.last_name}</h1></p>

                        <h2 style='color: #8672FF;'>Congratulations!</h2>
                        <p>You have been successfully registered as a <strong>Student</strong> on <strong>Quizzy</strong>.</p>
                        <p>Your roll number is <b>{stu.dept}-{stu.roll_num}</b> and you have been registered in <b>{stu.dept}</b> Department. Your date of admission is <b>{stu.addmission_year}</b></p>
                        <p>We are thrilled to have you join our learning community.</p>
                        <p></p>
                        <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Login to Quizzy</a>

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



            signUp_student signUp_Student = new signUp_student();
            signUp_Student.check(stu);
            return RedirectToAction("index");
        }



        [HttpPost]

        public IActionResult teacherReg(Teacher model)
        {

            string serverAddress = $"{Request.Scheme}://{Request.Host}";
            string loginUrl = $"{serverAddress}/login/index";
            ViewBag.Success = "Email Verified Successfully";
            string role = HttpContext.Session.GetString("UserRole");
            string email = HttpContext.Session.GetString("UserEmail");
            string password = HttpContext.Session.GetString("UserPassword");

            model.email = email;
            model.role = role;
            model.password = password;


            HttpContext.Session.Remove("UserRole");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserPassword");



            // Send confirmation email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
            message.To.Add(new MailboxAddress(model.first_name + " " + model.last_name, model.email));
            message.Subject = "Registration Successful - Quizzy";

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                        <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                            <p>Dear,<h1> {model.first_name} {model.last_name}</h1></p>

                            <h2 style='color: #8672FF;'>Congratulations!</h2>
                            <p>You have been successfully registered as a <strong>Teacher</strong> on <strong>Quizzy</strong>.</p>
                            <p>We are excited to have you as part of our community of educators.</p>

                            <p></p>
                            <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Login to Quizzy</a>

                            <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                <footer style='font-size: 12px; color: #888;'>Happy Teaching!</footer>
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
            Console.WriteLine($"name is {model.last_name} and email is {model.email}");
            return RedirectToAction("index");
        }


        [HttpPost]
        public IActionResult EnterOtp(string otpInput)
        {
            string savedOtp = HttpContext.Session.GetString("EmailOTP");
            string role = HttpContext.Session.GetString("UserRole");
            string email = HttpContext.Session.GetString("UserEmail");
            Console.WriteLine($"user with email {email} has entered otp {otpInput} and origanal otp is {savedOtp} and the role is {role}");
            if (savedOtp == otpInput)
            {
                
                // Success
                HttpContext.Session.Remove("EmailOTP"); // Clear session after success
                TempData["Success"] = "Email Verified Successfully";

                if (role == "student")
                {
                    

                    return RedirectToAction("stuReg");
                }
                else if (role == "teacher")
                {
                    return RedirectToAction("teacherReg");
                }
            }
            else
            {
                ViewBag.Error = "Invalid OTP.";
                return View("EnterOtp");
            }
            return View();
        }

    }

}
