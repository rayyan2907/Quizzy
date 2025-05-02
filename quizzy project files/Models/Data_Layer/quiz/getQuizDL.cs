using DBHelper;
using System.Data;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class getQuizDL
    {
        public static DataTable getQuiz(string id)
        {
            string query = $"select quiz_name,quizID,subject_name,subject_code,is_asssign from quiz q join subjects s on s.subjectID=q.subjectID where s.subjectID = {id}";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
