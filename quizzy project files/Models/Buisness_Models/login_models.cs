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
        public string role { get; set; }
    }
    public class Student : Registraton_models 
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string roll_num { get; set; }
        public string dept {  get; set; }
        public string addmission_year { get; set; }


    }

    public class Teacher : Registraton_models
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }


}
