using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.registration;
using Quizzy.Models.DataLayer_Models;

namespace Quizzy.Models.Buisness_Layer.registration
{
    public class SignupTeacher
    {
        public string reg (Buisness_Models.Teacher teacher)
        {
            RegistrationModel model = new RegistrationModel ();
            TeacherModel teac = new TeacherModel ();
            TeacherSignup signup = new TeacherSignup ();

            model.email=teacher.email;
            model.Password=teacher.password;
            model.role=teacher.role;

            teac.first_name=teacher.first_name;
            teac.last_name=teacher.last_name;
            teac.email=teacher.email;

            bool isStudentSave = signup.addTeacher(teac);


            if (!isStudentSave)
            {
                return "Error in Registration";
            }
            bool isLoginSave = signup.addLogin(model);


            if (isLoginSave && isStudentSave)
            {
                return "Registration Successfull";
            }
            else
            {
                return "Error in Registration";
            }
        }
    }
}
