using Itec_Mangement;
using System.Data;

namespace Quizzy.Models.Data_Layer
{
    public class login_check
    {
        public DataTable userCheck(Buisness_Models.login_models model) 
        { 
        string query = $"select role from login_details where email = '{model.email}' and password = '{model.password}'";
        DataTable DataTable = DatabaseHelper.Instance.GetData(query);
        return DataTable;
        }
    }

}
