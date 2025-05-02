using Quizzy.Models.Data_Layer.quiz;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class getQuizBL
    {
        public static DataTable getQuiz(string id )
        {
            return getQuizDL.getQuiz( id );
        }
    }
}
