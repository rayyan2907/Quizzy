using DBHelper;
using Quizzy.Models.Buisness_Models;
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

        public static quiz_model getQuizData(string id)
        {
            string query = $"select quiz_name,given_time,attempt,subjectID,isPublic,is_asssign from quiz where quizID={id};";
            quiz_model q = new quiz_model();

            DataTable dt = DatabaseHelper.Instance.GetData(query);
            q.quizName = dt.Rows[0]["quiz_name"].ToString();
            q.given_time = dt.Rows[0]["given_time"].ToString();
            q.attempt =Convert.ToBoolean( dt.Rows[0]["attempt"]);
            q.subID = dt.Rows[0]["subjectID"].ToString();
            q.isPublic = Convert.ToBoolean( dt.Rows[0]["isPublic"]);
            q.isAssign = Convert.ToBoolean(dt.Rows[0]["is_asssign"]);

            return q;


        }



    }
}
