using DBHelper;
using System.Data;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class stu_quizDL
    {
        public static DataTable getQuizes(string id)
        {
            string query = $"select quizID,isPublic,quiz_name,given_time,subjectID,is_asssign from quiz where subjectID={id} and is_asssign=true";
            return DatabaseHelper.Instance.GetData(query) ;
        }
        public static DataTable getopenQuizes()
        {
            string query = $"select quizID,isPublic,quiz_name,given_time,s.subject_name,s.subject_code,q.subjectID,is_asssign from quiz q join subjects s on s.subjectID=q.subjectID  where isPublic=true and is_asssign=true";
            return DatabaseHelper.Instance.GetData(query);
        }
        public static DataTable getResults(string id)
        {
            string query = $"select quiz_name, resultID, mcq_marks, shq_marks, total_marks, q.quizID, studentID from results r join quiz q on q.quizID=r.quizID where q.subjectID = {id}";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable getResultsOpenQuiz(string id)
        {
            string query = $"select quiz_name, resultID, mcq_marks, shq_marks, total_marks, q.quizID, studentID from results r join quiz q on q.quizID = r.quizID where r.studentID = {id} and q.isPublic= true";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable getattemptQuizes(string id)
        {
            string query = $"select q.quiz_name, count(a.quizID) as attempts, sum(mcq_marks)/count(a.quizID) as avg_mmcq, sum(shq_marks)/count(a.quizID) as avg_short, (sum(mcq_marks)+sum(shq_marks))/count(a.quizID) as avg_gained, sum(total_marks)/count(a.quizID) as avg_total from attempt a join quiz q on q.quizID=a.quizID join results r on r.quizID=a.quizID where a.studentID={id} group by a.quizID";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}