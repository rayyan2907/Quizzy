using System.Data;
using System.Globalization;
using Quizzy.Models.Data_Layer.quiz;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class checkQuizBL
    {
        public static DataTable showAllQuizzes(string id)
        {
            return checkQuizDL.showAllQuizzes(id);
        }
        public static DataTable studentQuizzes(int quizId)
        {
            return checkQuizDL.studentQuizzes(quizId);
        }
    }
}
