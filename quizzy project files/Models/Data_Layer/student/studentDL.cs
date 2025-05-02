using DBHelper;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.DataLayer_Models;
using System.Data;

namespace Quizzy.Models.Data_Layer.student
{
    public class studentDL
    {
        public Buisness_Models.Student getStu(string id)
        {
            string query = $"select studentID,first_name,last_name,dept,roll_num,addmission_year,email from students where studentID = '{id}'";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            Student s = new Student();
            s.stuID = id;
            s.first_name = dt.Rows[0]["first_name"].ToString();
            s.last_name = dt.Rows[0]["last_name"].ToString();
            s.dept = dt.Rows[0]["dept"].ToString();
            s.addmission_year = dt.Rows[0]["addmission_year"].ToString();
            s.email = dt.Rows[0]["email"].ToString();
            s.roll_num = dt.Rows[0]["roll_num"].ToString();

            return s;
        }

        public static DataTable getStuSub(string id)
        {
            string query = $"select e.subjectID,e.studentID,e.status,s.subject_name,s.subject_code,t.first_name,t.last_name,stu.first_name as stu_first,stu.last_name as stu_last,stu.roll_num,stu.addmission_year,stu.dept from enrollments e join subjects s on s.subjectID = e.subjectID join students stu on stu.studentID=e.studentID join teachers t on t.teacherID= s.teacherID where stu.studentID = {id}";
            return DatabaseHelper.Instance.GetData(query);

        }

        public static DataTable getAllStuSub()
        {
            string query = $"select subjectID,subject_name,t.teacherID,first_name,last_name,email,subject_code from subjects s join teachers t on t.teacherID = s.teacherID;";
            return DatabaseHelper.Instance.GetData(query);

        }

        public static bool enrollStu(enrollModel e)
        {
            string query = $"insert into enrollments (subjectID,StudentID,status) values ('{e.courseID}','{e.stuID}',{e.status})";
            int rows = DatabaseHelper.Instance.Update(query) ;
            return rows > 0;

        }

        public static DataTable checkIfEnrolled(Enrollment e)
        {
            string query = $"select * from enrollments where studentID = {e.stuID} and subjectID = {e.courseID}";
            return DatabaseHelper.Instance.GetData(query) ;
        }

        public static subject_model getSubject(string id)
        {
            string query = $"select subjectID,subject_name,subject_code,teacherID from subjects where subjectID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query);
            subject_model subj = new subject_model();
            subj.name = dt.Rows[0]["subject_name"].ToString();
            subj.subjectID = dt.Rows[0]["subjectID"].ToString();
            subj.teacherID = dt.Rows[0]["teacherID"].ToString();
            subj.code = dt.Rows[0]["subject_code"].ToString();
            return subj;
        }

        public static Teacher getTeacher(string id)
        {
            string query = $"select teacherID,first_name,last_name,email from teachers where teacherID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query) ;
            Teacher obj = new Teacher();
            obj.first_name=dt.Rows[0]["first_name"].ToString();
            obj.last_name= dt.Rows[0]["last_name"].ToString();
            obj.email= dt.Rows[0]["email"].ToString();

            return obj;
        }

        public static DataTable viewReq(string id)
        {
            string query = $"select s.studentID,s.first_name,s.last_name,e.status,s.roll_num,s.dept,s.addmission_year,s.email from enrollments e join students s on s.studentID=e.studentID where subjectID={id}";
            return DatabaseHelper.Instance.GetData(query) ;
        }

        public static bool updateEnroll(Enrollment e)
        {
            string query = $"update enrollments set  status={e.status} where studentID={e.stuID} and subjectID={e.courseID}";
            int rows = DatabaseHelper.Instance.Update(query) ;
            return rows> 0 ;
        }
        public static bool unEnroll(Enrollment e)
        {
            string query = $"delete from enrollments where studentID={e.stuID} and subjectID={e.courseID}";
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        public static DataTable statsEnroll(string id)
        {
            string query = $"select count(subjectID) as enroll from enrollments where studentID={id} and status = true";
            return DatabaseHelper.Instance.GetData(query) ;
        }
        public static DataTable statsCompQuiz(string id)
        {
            string query = $"select count(atemptID) as comp_quiz from attempt where studentID = {id}";
            return DatabaseHelper.Instance.GetData(query);
        }
        public static DataTable statsUpcomingQuiz(string id)
        {
            string query = $"select count(q.quizID) as upcoming from enrollments e join subjects s on s.subjectID=e.subjectID join quiz q on q.subjectID=s.subjectID where studentID={id} and q.is_asssign=true";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable statsAggregate(string id)
        {
            string query = $"select (sum(mcq_marks)+sum(shq_marks))/sum(total_marks)*100 as agrregate from results where studentID={id}";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
