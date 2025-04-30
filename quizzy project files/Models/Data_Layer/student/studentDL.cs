using DBHelper;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.student
{
    public class studentDL
    {
        public Buisness_Models.Student getStu(string id)
        {
            string query = $"select studentID,first_name,last_name,dept,roll_num,addmission_year,email from students where studentID = '{id}'";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            Student s = new Student();
            s.stuID = id;
            s.first_name = dt.Rows[0]["first_name"].ToString();
            s.last_name = dt.Rows[0]["last_name"].ToString();
            s.dept = dt.Rows[0]["dept"].ToString();
            s.addmission_year = dt.Rows[0]["addmission_year"].ToString();
            s.email = dt.Rows[0]["email"].ToString();
            s.roll_num = dt.Rows[0]["roll_num"].ToString();

            return s;
        }
    }
}
