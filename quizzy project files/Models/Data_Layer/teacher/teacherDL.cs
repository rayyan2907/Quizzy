using DBHelper;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.teacher
{
    public class teacherDL
    {
        public Buisness_Models.Teacher getTeac(string id)
        {
            string query = $"select teacherID,first_name,last_name,email from teachers where teacherID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            Teacher t= new Teacher();
            t.teachID = id;
            t.first_name = dt.Rows[0]["first_name"].ToString();
            t.last_name = dt.Rows[0]["last_name"].ToString();
            t.email = dt.Rows[0]["email"].ToString();

            return t;
        }

        public static DataTable announcement(string id)
        {
            string query = $"select s.first_name,s.last_name,s.email from enrollments e join students  s on s.studentID = e.studentID where subjectID ={id} and status = true";

            return DatabaseHelper.Instance.GetData(query);
        }



        public static DataTable statsTotalStu(string id)
        {
            string query = $"select count(studentID) as total_stu from enrollments where subjectID = {id} and status=true ";
            return DatabaseHelper.Instance.GetData(query);
        }
        public static DataTable statsCompQuiz(string id)
        {
            string query = $"select count(quizID) as quizes from quiz q join subjects s on s.subjectID=q.subjectID where s.subjectID = {id} and q.is_asssign=true";
            return DatabaseHelper.Instance.GetData(query);
        }
        public static DataTable statsUpcomingQuiz(string id)
        {
            string query = $"select count(quizID) as quizes from quiz q join subjects s on s.subjectID=q.subjectID where s.subjectID = {id} and q.is_asssign=false";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable statsAggregate(string id)
        {
            string query = $"select ((sum(mcq_marks)+sum(shq_marks))/sum(total_marks))*100 as aggregate from results r join quiz q on q.quizID=r.quizID where subjectID={id} ";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
