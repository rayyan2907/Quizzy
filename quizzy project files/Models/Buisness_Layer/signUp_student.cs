namespace Quizzy.Models.Buisness_Layer
{
    public class signUp_student
    {
        public void check (Models.Buisness_Models.Student stu)
        {
            Console.WriteLine($"a new student with email {stu.email} and name as {stu.last_name} and password {stu.password} has sign up on date {stu.addmission_year} and has role {stu.role}");
        }
         
        
    }
}
