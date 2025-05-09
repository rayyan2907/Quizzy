using Quizzy.Models.Buisness_Models;

namespace Quizzy.Models.DataLayer_Models
{
    public class StudentModel
    {
        public int studentID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int roll_num { get; set; }
        public string dept { get; set; }
        public int year { get; set; }
        public string email { get; set; }
    }
}