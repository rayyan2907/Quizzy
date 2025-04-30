using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.student;

namespace Quizzy.Models.Buisness_Layer.student
{
    public class StudentBL
    {
        public static Models.Buisness_Models.Student getData(string id)
        {   
            studentDL model = new studentDL();

            Student student = new Student();
            student = model.getStu(id);

            return student;

        }
    }
}
