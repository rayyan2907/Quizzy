using MimeKit.Tnef;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.DataLayer_Models;
using Microsoft.AspNetCore.Identity;
using Quizzy.Models.Data_Layer.registration;

namespace Quizzy.Models.Buisness_Layer.registration
{
    public class signUp_student
    {
        public string reg(Buisness_Models.Student stu)   
        {
            Student_signup signup = new Student_signup();
            RegistrationModel login = new RegistrationModel();
            StudentModel student = new StudentModel();

            int roll_number;
            bool isRoll = int.TryParse(stu.roll_num, out roll_number);
            if (!isRoll)
            {
                return "Roll number must be in numbers";
            }
            student.roll_num = roll_number;

           
            int year = Convert.ToInt32(stu.addmission_year);

            student.email = stu.email;
            student.dept = stu.dept;
            student.year = year;
            student.first_name = stu.first_name;
            student.last_name = stu.last_name;

            login.email = stu.email;
            login.role = stu.role;
            login.Password = stu.password;

            bool isStudentSave = signup.addStudent(student);
            if (!isStudentSave)
            {
                return "Please Fill the data correctly";
            }

            bool isLoginSave = signup.addLogin(login);

            if (isLoginSave && isStudentSave)
            {
                return "Registration Successfull";
            }
            else
            {
                return "Please Fill the data correctly";
            }
        }
    }
}
