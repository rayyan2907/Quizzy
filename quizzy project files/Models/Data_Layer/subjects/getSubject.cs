using DBHelper;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.subjects
{
    public class getSubject
    {
        public static DataTable getSub(string id)
        {
            string query = $"select subjectID,subject_name,subject_code from subjects where teacherID = {id}";
            return DatabaseHelper.Instance.GetData(query) ;
        }

        public static subject_model getSubFromID(string id)
        {
            string query = $"select subjectID,subject_name,subject_code from subjects where subjectID = {id}";
            subject_model subject = new subject_model();
            DataTable dt = DatabaseHelper.Instance.GetData(query);
            subject.code = dt.Rows[0]["subject_code"].ToString();
            subject.name = dt.Rows[0]["subject_name"].ToString();
            subject.subjectID = id;
            return subject ;
            
        }
    }
}
