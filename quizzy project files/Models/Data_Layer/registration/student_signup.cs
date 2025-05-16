using DBHelper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Quizzy.Models.Data_Layer.registration
{
    public class Student_signup
    {
        public bool addLogin(DataLayer_Models.RegistrationModel reg,out string errormsg)
        {
            errormsg=string.Empty;
            try
            {
                string query = $"insert into login_details (email,password,role) values('{reg.email}','{reg.Password}','{reg.role}')";
                int rows = DatabaseHelper.Instance.Update(query);
                return rows > 0;
            }
            catch(Exception ex)
            {
                errormsg= ex.Message;
                return false;

            }
        }

        public bool addStudent(DataLayer_Models.StudentModel stu) 
        {
            string query = $"insert into students (first_name,last_name,roll_num,dept,addmission_year,email) values ('{stu.first_name}','{stu.last_name}','{stu.roll_num}','{stu.dept}','{stu.year}','{stu.email}')";
            Console.WriteLine(query);
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;

        }
    }
}
