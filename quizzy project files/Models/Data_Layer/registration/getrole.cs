using DBHelper;
using System.Data;

namespace Quizzy.Models.Data_Layer.registration
{
    public class Getrole
    {
        public DataTable user_get(Buisness_Models.Registraton_models registraton)
        {
            string query = $"select role from login_details where email = '{registraton.email}'";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
