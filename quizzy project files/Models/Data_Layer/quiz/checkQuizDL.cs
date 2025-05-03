using System.Data;
using DBHelper;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class checkQuizDL
    {
        public static DataTable showAllQuizzes(string subject_id)
        {
            string query = $"SELECT quizID,quiz_name FROM quiz q JOIN subjects s ON q.subjectID = '{subject_id}'";
            
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
