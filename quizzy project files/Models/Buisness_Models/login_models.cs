using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Quizzy.Models.Buisness_Models
{
    public class login_models
    {
        public string email {  get; set; }
        public string password { get; set; }
    }
    public class Registraton_models
    {
        public string email { get; set; }
        public string password { get; set; }
        public string cnfrm_pwd { get; set; }
        public string role { get; set; }
    }
    public class Student : Registraton_models 
    {
        public string stuID  { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string roll_num { get; set; }
        public string dept {  get; set; }
        public string addmission_year { get; set; }
    }

    public class Teacher : Registraton_models
    {
        public string teachID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class verify_otp
    {
        public string Otp { get; set; }
        public bool isVerified { get; set; }
    }
    public class change_pass
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string email { get; set; }

        public string otp1 { get; set; }
        public string otp2 { get; set; }
        public string otp3 { get; set; }
        public string otp4 { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("newPassword", ErrorMessage = "The passwords do not match")]
        public string confirmPassword { get; set; }
    }


}
