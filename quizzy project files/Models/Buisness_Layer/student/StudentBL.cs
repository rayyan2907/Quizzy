using Microsoft.AspNetCore.StaticFiles;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.quiz;
using Quizzy.Models.Data_Layer.student;
using Quizzy.Models.DataLayer_Models;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.student
{
    public class StudentBL
    {
        public static Models.Buisness_Models.Student getData(string id)
        {   
            studentDL model = new studentDL();
            Student student = new Student();
            student = model.getStu(id);

            return student;

        }
        public static DataTable getStuCourses(string id)
        {
            DataTable dt = studentDL.getStuSub(id);
            return dt;
        }
        public static DataTable getAllStuCourses()
        {
            DataTable dt = studentDL.getAllStuSub();
            return dt;
        }

        public static bool stuEnroll(Enrollment e)
        {
            enrollModel em = new enrollModel(); 
            em.status = e.status;
            int stuID;
            bool flag = int.TryParse(e.stuID, out stuID);
            int courseId;
            bool flag2 = int.TryParse(e.courseID, out courseId);

            if (!flag2 || !flag) 
            {
                return false;
            }
            em.stuID = stuID;
            em.courseID = courseId;
            return studentDL.enrollStu(em);
            
        }
        public static bool CheckIfEnrolled(Enrollment e)
        {
            DataTable dt = studentDL.checkIfEnrolled(e);
            if (dt == null || dt.Rows.Count==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static subject_model getSub(string id)
        {
            subject_model subjectModel = new subject_model();
            subjectModel = studentDL.getSubject(id);

            return subjectModel;
        }
        public static Teacher getTec(string id)
        {
            Teacher t = new Teacher();
            t= studentDL.getTeacher(id);
            return t;
        }

        public static DataTable viewReq(string id)
        {
            DataTable dt = studentDL.viewReq(id);
            return dt;
        }

        public static bool updateAssign(Enrollment e)
        {
            return studentDL.updateEnroll(e);
        }
        public static bool deleteAssign(Enrollment e)
        {
            return studentDL.unEnroll(e);
        }
        public static DataTable statsEnroll(string id)
        {
            return studentDL.statsEnroll(id);
        }
        public static DataTable statsComplete(string id)
        {
            return studentDL.statsCompQuiz(id);
        }
        public static DataTable statsUpcomming(string id)
        {
            return studentDL.statsUpcomingQuiz(id);
        }
        public static DataTable statsAvg(string id)
        {
            return studentDL.statsAggregate(id);
        }
    }
}
