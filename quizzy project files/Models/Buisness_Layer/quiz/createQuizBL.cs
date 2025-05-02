using Quizzy.Models.Data_Layer.quiz;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class createQuizBL
    {
        public static bool deleteQuiz(string id)
        {
            return createQuizDL.deleteQuiz(id);
        }

        public static bool assignQuiz(string id)
        {
            return createQuizDL.assignQuiz(id);
        }

        public static bool unassignQuiz(string id)
        {
            return createQuizDL.assignQuiz(id);
        }
    }
}