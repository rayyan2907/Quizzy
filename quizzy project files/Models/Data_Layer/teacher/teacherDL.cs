using DBHelper;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.teacher
{
    public class teacherDL
    {
        public Buisness_Models.Teacher getTeac(string id)
        {
            string query = $"select teacherID,first_name,last_name,email from teachers where teacherID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            Teacher t= new Teacher();
            t.teachID = id;
            t.first_name = dt.Rows[0]["first_name"].ToString();
            t.last_name = dt.Rows[0]["last_name"].ToString();
            t.email = dt.Rows[0]["email"].ToString();

            return t;
        }
    }
}
