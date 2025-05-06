using System.Data;
using DBHelper;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class checkQuizDL
    {
        public static DataTable showAllQuizzes(string subject_id)
        {
            string query = $"SELECT quizID,quiz_name, is_asssign FROM quiz q JOIN subjects s ON q.subjectID = '{subject_id}'";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable studentQuizzes(int id)
        {
            string query = "SELECT CONCAT(s.addmission_year, '-', s.dept, '-', s.roll_num) AS registration_number, CONCAT(s.first_name, ' ', s.last_name) AS name, COUNT(CASE WHEN m.correct_opt = a.answer THEN 1 END) AS marks FROM students s JOIN mcq_answers a ON s.studentID = a.studentID JOIN mcqs m ON m.mcqID = a.mcqID GROUP BY s.studentID, s.addmission_year, s.dept, s.roll_num, s.first_name, s.last_name;";
            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
