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
        public static DataTable studentQuizzes(string quizId)
        {
            return checkQuizDL.studentQuizzes(quizId);
        }

        public static DataTable AnswersOfStudent(string quizID, string studentId)
        {
            return checkQuizDL.AnswersOfStudent(quizID, studentId);
        }

        public static bool AssignGradeToShortAnswer(string studentId, string shqID, decimal marks)
        {
            return checkQuizDL.AssignGradeToShortAnswer(studentId, shqID, marks);
        }

        public static DataTable GetFinalResults(string id)
        {
            return checkQuizDL.GetFinalResults(id);
        }

        public static bool SaveOrUpdateResult(int studentId, string quizId, int mcqMarks, int shqMarks, int totalMarks)
        {
            return checkQuizDL.SaveOrUpdateResult(studentId, quizId, mcqMarks, shqMarks, totalMarks);
        }
    }
}
