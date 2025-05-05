using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;

namespace Quizzy.Controllers.login
{
    public class ForgotPasswordController : Controller
    {
        private static Dictionary<string, string> OtpStore = new Dictionary<string, string>();

        public IActionResult Index()
        {
            return View("forget_password");
        }

        public IActionResult changePass()
        {
            return View("forget_password");
        }

        // POST: /login/changePass (Email submission)
        [HttpPost]
        public IActionResult changePass(change_pass model)
        {
            if (string.IsNullOrEmpty(model.email))
            {
                TempData["Error"] = "Please enter your email address";
                return View("forget_password", model);
            }

            // Check if email exists in your database
            // This is placeholder code - replace with your actual user verification
            if (!UserExists(model.email))
            {
                TempData["Error"] = "Email not found in our records";
                return View("forget_password", model);
            }

            // Generate and send OTP
            string otp = GenerateOTP();
            OtpStore[model.email] = otp;

            // Send email with OTP
            bool emailSent = SendOTPEmail(model.email, otp);

            if (!emailSent)
            {
                TempData["Error"] = "Failed to send reset code. Please try again.";
                return View("forget_password", model);
            }

            // Show OTP form
            ViewBag.ShowOtp = true;
            ViewBag.Email = model.email;
            TempData["Message"] = "Reset code sent to your email";

            return View("forget_password", model);
        }

        // POST: /login/resetpassword (OTP verification and password reset)
        [HttpPost]
        [Route("login/resetpassword")]
        public IActionResult ResetPassword(change_pass model)
        {
            string email = model.email;

            // Combine OTP digits
            string submittedOtp = model.otp1 + model.otp2 + model.otp3 + model.otp4;

            // Validate OTP
            if (!OtpStore.ContainsKey(email) || OtpStore[email] != submittedOtp)
            {
                ViewBag.ShowOtp = true;
                ViewBag.Email = email;
                TempData["Error"] = "Invalid code. Please try again.";
                return View("forget_password", model);
            }

            // Validate passwords match (already done in model validation but double-checking)
            if (model.newPassword != model.confirmPassword)
            {
                ViewBag.ShowOtp = true;
                ViewBag.Email = email;
                TempData["Error"] = "Passwords do not match";
                return View("forget_password", model);
            }

            // Reset password in your database
            bool passwordResetSuccess = ResetUserPassword(email, model.newPassword);

            if (!passwordResetSuccess)
            {
                ViewBag.ShowOtp = true;
                ViewBag.Email = email;
                TempData["Error"] = "Password reset failed. Please try again.";
                return View("forget_password", model);
            }

            // Remove OTP from store
            OtpStore.Remove(email);

            // Redirect to login with success message
            TempData["Check"] = "Password reset successful. You can now log in with your new password.";
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        // Helper methods
        private bool UserExists(string email)
        {
            // Replace with actual user verification from your database
            // Example: return _userService.UserExists(email);
            return true; // Placeholder
        }

        private string GetUserNameByEmail(string email)
        {
            // Implement this method to fetch the user's name from your database
            // For now returning a placeholder
            return "Quizzy User";
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SendOTPEmail(string email, string otp)
        {
            try
            {
                // Create the email message in the same style as your registration email
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Quizzy - Modern Quiz System", "mrayyan403@gmail.com"));
                message.To.Add(new MailboxAddress("Quizzy User", email));
                message.Subject = "Password Reset Code - Quizzy";

                // Get the user's details if possible
                string userName = GetUserNameByEmail(email); // Implement this method to get user name

                // Generate the reset URL (in a real implementation, you would use the actual URL)
                string resetUrl = $"{Request.Scheme}://{Request.Host}/login";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; background-color: white; color: #2E2B41; padding: 30px;'>
                        <div style='background-color: #F3F0FF; border-radius: 10px; padding: 30px; max-width: 600px; margin: auto;'>
                            <p>Dear User,</p>
                            <h2 style='color: #8672FF;'>Password Reset Request</h2>
                            <p>We received a request to reset your password for your <strong>Quizzy</strong> account.</p>
                            <p>Your password reset code is:</p>
                            <div style='text-align: center; margin: 30px 0;'>
                                <h1 style='letter-spacing: 8px; font-size: 32px; color: #8672FF; font-weight: bold; padding: 15px; background-color: white; border-radius: 10px; display: inline-block;'>{otp}</h1>
                            </div>
                            <p>This code will expire in 10 minutes. If you did not request a password reset, please ignore this email or contact support.</p>
                            <div style='margin-top: 40px; font-size: 12px; color: #999; text-align: center;'>
                                <footer style='font-size: 12px; color: #888;'>Thank you for using Quizzy!</footer>
                                &copy; 2025 Quizzy. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>"
                };

                // Send the email using the same SMTP client settings as your registration
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("mrayyan403@gmail.com", "yuax ekty ofav lkvj"); // your app password
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        private bool ResetUserPassword(string email, string newPassword)
        {
            // Replace with your password reset logic
            // Example: return _userService.ResetPassword(email, newPassword);
            return true; // Placeholder
        }
    }
}