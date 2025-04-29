using DBHelper;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.registration
{
    public class Login
    {
        public DataTable user_get(login_models log)
        {
            string query = $"select role from login_details where email = '{log.email}'";
            return DatabaseHelper.Instance.GetData(query);
        }

        public DataTable getPwd(login_models log)
        {
            string query = $"select password from login_details where email = '{log.email}'";
            return DatabaseHelper.Instance.GetData(query);
        }

        public DataTable getStuData(string email)
        {
            string query = $"select * from students where email = '{email}'";
            return DatabaseHelper.Instance.GetData(query);
        }

        public DataTable getTeaData(string email)
        {
            string query = $"select * from teachers where email = '{email}'";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
