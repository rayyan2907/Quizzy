using Quizzy.Models.Data_Layer.registration;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.registration
{
    public class Signup
    {
        public DataTable check(Buisness_Models.Registraton_models registraton)
        {
            Getrole getrole = new Getrole();
            DataTable dt = getrole.user_get(registraton);
            return dt;
        }
    }
}
