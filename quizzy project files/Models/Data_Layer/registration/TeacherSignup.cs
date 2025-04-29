using DBHelper;


namespace Quizzy.Models.Data_Layer.registration
{
    public class TeacherSignup

    {
        
        public bool addLogin(DataLayer_Models.RegistrationModel reg)
        {
            string query = $"insert into login_details (email,password,role) values ('{reg.email}','{reg.Password}','{reg.role}')";
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        public bool addTeacher(DataLayer_Models.TeacherModel teacher)
        {
            string query = $"insert into teachers (first_name, last_name, email) values ('{teacher.first_name}','{teacher.last_name}','{teacher.email}')";
            
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;

        }
    }
}
