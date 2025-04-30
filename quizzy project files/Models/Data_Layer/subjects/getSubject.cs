using DBHelper;
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
    }
}
