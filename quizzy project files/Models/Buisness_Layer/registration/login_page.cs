using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.registration;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.registration
{

    public class login_page
    {
        Login log = new Login();

        public bool userExists(login_models model)
        {
            DataTable dt = log.user_get(model);

            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string GetPwd(login_models model)
        {
            DataTable dt = log.getPwd(model);
            return dt.Rows[0]["password"].ToString();
        }

        public DataTable getUserData(login_models model)
        {
            DataTable dt = log.user_get(model);
            DataTable user = new DataTable();
            string role = dt.Rows[0]["role"].ToString();

            if (role == "student")
            {
                user = log.getStuData(model.email);
            }
            else if (role == "teacher")
            {
                user = log.getTeaData(model.email);
            }
            return user;
        }
        public string getUserRole(login_models model)
        {
            DataTable dt = log.user_get(model);
            return dt.Rows[0]["role"].ToString();
        }



    }
}
