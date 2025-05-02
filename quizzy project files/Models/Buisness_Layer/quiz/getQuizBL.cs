using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.quiz;
using Quizzy.Models.DataLayer_Models;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class getQuizBL
    {
        public static DataTable getQuiz(string id )
        {
            return getQuizDL.getQuiz( id );
        }

        public static quiz_model getQuizdata(string id)
        {
            return getQuizDL.getQuizData( id );
        }

      
        
    }
}
