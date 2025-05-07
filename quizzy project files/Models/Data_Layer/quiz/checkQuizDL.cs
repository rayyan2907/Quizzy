using System.Data;
using DBHelper;
using MySql.Data.MySqlClient;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class checkQuizDL
    {
        public static DataTable showAllQuizzes(string subject_id)
        {
            string query = $"SELECT quizID,quiz_name, is_asssign FROM quiz q JOIN subjects s ON q.subjectID = '{subject_id}'";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable studentQuizzes(int quizId)
        {
            string query = $@"
                SELECT 
                    CONCAT(s.addmission_year, '-', s.dept, '-', s.roll_num) AS registration_number, 
                    CONCAT(s.first_name, ' ', s.last_name) AS name, 
                    COUNT(CASE WHEN m.correct_opt = a.answer THEN 1 END) AS mcqs_marks,
                    IFNULL(SUM(sc.marks), 0) AS sqs_marks
                FROM attempt apt
                INNER JOIN students s ON s.studentID = apt.studentID
                LEFT JOIN mcq_answers a ON a.studentID = s.studentID
                LEFT JOIN mcqs m ON m.mcqID = a.mcqID AND m.quizID = apt.quizID
                LEFT JOIN shq_check sc ON sc.studentID = s.studentID
                LEFT JOIN short_questions sq ON sq.shqID = sc.shqID AND sq.quizID = apt.quizID
                WHERE apt.quizID = 9
                GROUP BY s.studentID, s.addmission_year, s.dept, s.roll_num, s.first_name, s.last_name;"; 

            return DatabaseHelper.Instance.GetData(query);
        }

    }
}
