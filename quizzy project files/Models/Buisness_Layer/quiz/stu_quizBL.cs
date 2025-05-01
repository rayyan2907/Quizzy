using Quizzy.Models.Data_Layer.quiz;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class stu_quizBL
    {
        public static DataTable getquiz(string id)
        {
            return stu_quizDL.getQuizes(id);
        }

        public static DataTable getopenquiz()
        {
            return stu_quizDL.getopenQuizes();


        }
    }
}
