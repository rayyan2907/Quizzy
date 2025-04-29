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

}
