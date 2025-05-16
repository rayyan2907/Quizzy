using Quizzy.Models.Data_Layer.quiz;
using Quizzy.Models.Data_Layer.student;
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
        public static DataTable getresults(string id,string id2)
        {
            return stu_quizDL.getResults(id,id2);
        }
        public static DataTable getresultsOpenQuiz(string id)
        {
            return stu_quizDL.getResultsOpenQuiz(id);
        }
        public static DataTable getattemptquiz(string id)
        {
            return stu_quizDL.getattemptQuizes(id);
        }
    }
}
