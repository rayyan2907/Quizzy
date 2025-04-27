using Quizzy.Models.Data_Layer;
using System.Data;

namespace Quizzy.Models.Buisness_Layer
{
    
    public class login_page
    {
        login_check login_Check = new login_check();

        public void login(Buisness_Models.login_models model)
        {

            DataTable dt = login_Check.userCheck(model);

            if (dt == null || dt.Rows.Count==0)
            {
                Console.WriteLine($"A user with email: {model.email} was not found");
            }
            else if (dt.Rows.Count == 1)
            {
                string role = dt.Rows[0]["role"].ToString();
                Console.WriteLine($"The user with email {model.email} and user role {role} has just loged in the system");

            }
        }

        
    }
}
