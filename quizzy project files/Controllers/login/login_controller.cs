using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Ocsp;
using Quizzy.Models.Buisness_Layer.registration;
using Quizzy.Models.Buisness_Models;
using System.Data;

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
            if (model == null)
            {
                TempData["log"] = "Session loading error";
                return RedirectToAction("index");
            }
            Console.WriteLine($"A user with email: {model.email} just tried to login");
            login_page login = new login_page();
            bool flag = login.userExists(model);
            if (!flag)
            {
                Console.WriteLine($"no user found with email {model.email}");
                TempData["log"] = "No user found";
                return RedirectToAction("index");



            }
            else
            {
                string pwd = login.GetPwd(model);

                if (model.password != pwd)
                {
                    Console.WriteLine($"User with email {model.email} has entered incorrect password");

                    TempData["log"] = "Incorrect Password";
                    return RedirectToAction("index");
                }
                else
                {
                    DataTable user = login.getUserData(model);
                    string first_name = user.Rows[0]["first_name"].ToString();
                    string last_name = user.Rows[0]["last_name"].ToString();
                    string email = user.Rows[0]["email"].ToString();
                    string role = login.getUserRole(model);
                    

                    

                    Console.WriteLine($"User with email {model.email} and name {first_name} {last_name} and role {role} has just log in to the system");


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                    message.To.Add(new MailboxAddress(first_name + " " + last_name, email));
                    message.Subject = "Log In Alert - Quizzy";

                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $@"
                    
                            <html>
                            <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                                <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                    <h2 style='color: #8672FF; text-align: center;'>Login Alert</h2>
                                    <p>Dear,<h1>{first_name} {last_name}</h1></p>
                                    <p>You have successfully logged in to your <strong>Quizzy</strong> account on <b>{DateTime.Now}</b>.</p>
                                    <p>If this wasn't you, please log in and change your password immediately.</p>
                                    <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                        <footer style='font-size: 12px; color: #888;'>Stay Secure!</footer>
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

                    if (role == "student")
                    {

                        HttpContext.Session.SetString("UserId", user.Rows[0]["studentID"].ToString());
                        Response.Cookies.Append("UserId", user.Rows[0]["studentID"].ToString(), new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(7),
                            IsEssential = true
                        });


                        return RedirectToAction("main", "student");

                    }
                    else if (role == "teacher")
                    {

                        HttpContext.Session.SetString("teacId", user.Rows[0]["teacherID"].ToString());
                        Response.Cookies.Append("teacId", user.Rows[0]["teacherID"].ToString(), new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(7),
                            IsEssential = true
                        });


                        return RedirectToAction("main", "teacher");

                    }
                   

                }
                return RedirectToAction("index");

            }




        }
        [HttpPost]

        public IActionResult register(Registraton_models reg)
        {
            Console.WriteLine($"user with email {reg.email} and role {reg.role} is trying to sign up");
            Signup signup = new Signup();
            DataTable dt = signup.check(reg);


            if (dt == null || dt.Rows.Count == 0)
            {

                if (reg.password != reg.cnfrm_pwd)
                {
                    TempData["ErrorMessage"] = "Passwords does not match!";
                    return RedirectToAction("register");


                }
                else
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
                            <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                                <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                                    <h2 style='color: #8672FF; text-align: center;'>Verify Your Email</h2>
                                    <p style='font-size: 16px; text-align: center;'>Use the following OTP to verify your email:</p>

                                    
                                    <div style='font-size: 24px; font-weight: bold; margin: 20px auto; display: block; text-align: center; padding: 15px 30px; border: 2px dashed #8672FF; background-color: #ffffff; color: #2E2B41; border-radius: 5px;'>
                                        {generatedOtp}
                                    </div>


                                    <p style='font-size: 14px; color: #555; text-align: center;'>This OTP will expire soon. Please verify quickly. </p>

                                    <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                        <footer style='font-size: 12px; color: #888;'>If you did not request this verification, please ignore this email.</footer>
                                        &copy; 2025 Quizzy. All rights reserved.
                                    </div>
                                </div>
                            </body>
                            </html>
"

                    };
                    try
                    {
                        // (Then use SMTP to send the message)
                        using (var client = new MailKit.Net.Smtp.SmtpClient())
                        {
                            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                            client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // NOT your email password! Use Gmail App Password
                            client.Send(message);
                            client.Disconnect(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["log"] = "Internet not connected";
                        Console.WriteLine("internet issue");
                        return RedirectToAction("index");
                    }

                    TempData["otp"] = "Please check your spam mail for OTP";

                    return RedirectToAction("EnterOtp");

                }
            }
            else
            {
                Console.WriteLine($"User with email {reg.email} already exists as a {dt.Rows[0]["role"].ToString()}");

                TempData["ErrorMessage"] = $"User already exists as a {dt.Rows[0]["role"].ToString()}";

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




            
            signUp_student signUp_Student = new signUp_student();
            string msg = signUp_Student.reg(stu);
            
            if (msg == "Registration Successfull")
            {
                TempData["Check"] = msg;

                // Send confirmation email
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(stu.first_name + " " + stu.last_name, stu.email));
                message.Subject = "Student Registration Successful - Quizzy";

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
                    return RedirectToAction("register");

                }



            }
            else 
            {
                TempData["Check"] = msg;
                return View("student_reg", stu);


            }
            Console.WriteLine($"New student with name {stu.first_name} {stu.last_name} has signed up");

            return RedirectToAction("index");
        }



        [HttpPost]

        public IActionResult teacherReg(Teacher model)
        {
            SignupTeacher teacher = new SignupTeacher();

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

            string msg = teacher.reg(model);

            if (msg == "Registration Successfull")
            {
                TempData["Check"] = msg;

                // Send confirmation email
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress(model.first_name + " " + model.last_name, model.email));
                message.Subject = "Teacher Registration Successful - Quizzy";

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
                                <div style='text-align: center; margin-top: 20px;'>
                                    <a href='{loginUrl}' style='background-color: #8672FF; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-size: 16px;'>Login to Quizzy</a>
                                </div>

                            <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                <footer style='font-size: 12px; color: #888;'>Happy Teaching!</footer>
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
                    TempData["log"] = "Internet not connected";
                    Console.WriteLine("internet issue");
                    return RedirectToAction("register");

                }
            }

            else
            {
                TempData["Check"] = msg;
                return View("teacher_reg",model);


            }
            Console.WriteLine($"A new teacher with name {model.first_name} {model.last_name} has signup");

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
