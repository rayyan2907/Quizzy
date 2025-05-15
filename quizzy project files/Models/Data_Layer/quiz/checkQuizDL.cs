﻿using System.Data;
using System.Globalization;
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

        public static DataTable studentQuizzes(string quizID)
        {
            if (quizID == null)
            {
                Console.WriteLine("Null Quiz ID");
            }
            else
            {
                Console.WriteLine($"checkQuiz DL has quizID = {quizID}");
            }

            string query = $"SELECT * FROM quiz_results_view WHERE quizID = {quizID};";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable AnswersOfStudent(string quizID, string studentID)
        {
            string query = $@"
                SELECT shqID, sq.question AS question,
                       sa.answer AS answer
                FROM shq_answers sa 
                JOIN short_questions sq USING(shqID)
                WHERE sa.studentID = {studentID} AND sq.quizID = {quizID};";

            return DatabaseHelper.Instance.GetData(query);
        }

        public static bool AssignGradeToShortAnswer(string studentId, string shqID, decimal marks)
        {
            string query = $@"INSERT INTO shq_check (shqID, studentID, marks)
                              VALUES ({shqID}, {studentId}, {marks}) AS new
                              ON DUPLICATE KEY UPDATE marks = new.marks;";

            return DatabaseHelper.Instance.Update(query) > 0;
        }

        public static DataTable GetFinalResults(string quizId)
        {
            string query = @"SELECT studentID, mcqs_marks, sqs_marks, total_marks";

            return DatabaseHelper.Instance.GetData(query);
        }

        public static bool SaveOrUpdateResult(int studentId, string quizId, int mcqMarks, int shqMarks, int totalMarks)
        {
            string query = $@"INSERT INTO results (studentID, quizID, mcq_marks, shq_marks, total_marks, is_sent)
                              VALUES ({studentId}, {quizId}, {mcqMarks}, {shqMarks}, {totalMarks}, 0)
                              ON DUPLICATE KEY UPDATE 
                                  mcq_marks = {mcqMarks},
                                  shq_marks = {shqMarks},
                                  total_marks = {totalMarks},
                                  is_sent = 0;";

            return DatabaseHelper.Instance.Update(query) > 0;
        }
    }
}
