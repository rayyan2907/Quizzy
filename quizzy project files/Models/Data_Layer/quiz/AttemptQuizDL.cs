// Quizzy.Models.Data_Layer.quiz.AttemptQuizDL.cs
using DBHelper;
using Quizzy.Models.DataLayer_Models;
using Quizzy.Models.Buisness_Models;
using System;
using System.Data;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class AttemptQuizDL
    {
        // Load quiz for student attempt
        public static DataTable GetQuizMcqs(string quizId)
        {
            string query = $"SELECT mcqID, statement, option_A, option_B, option_C, option_D, correct_opt FROM mcqs WHERE quizID = {quizId}";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable GetQuizShqs(string quizId)
        {
            string query = $"SELECT shqID, question FROM short_questions WHERE quizID = {quizId}";
            return DatabaseHelper.Instance.GetData(query);
        }

        // Create a new attempt record
        public static bool CreateAttempt(attemptModel attempt)
        {
            string query = $"INSERT INTO attempt (quizID,studentID) VALUES ({attempt.quizID},{attempt.studentID})";
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        // Get the recently created attempt ID
        public static int GetLatestAttemptId(int studentId, int quizId)
        {
            string query = $"SELECT atemptID FROM attempt WHERE studentID = {studentId} AND quizID = {quizId} ORDER BY atemptID DESC LIMIT 1";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["atemptID"]);
            }
            return -1;
        }

        // Save MCQ answer
        public static bool SaveMcqAnswer(mcqAnswerModel answer)
        {
            // Check if answer exists already and update if it does
            string checkQuery = $"SELECT COUNT(*) FROM mcq_answers WHERE mcqID = {answer.mcqID} AND studentID = {answer.studentID}";
            DataTable dt = DatabaseHelper.Instance.GetData(checkQuery);
            int count = Convert.ToInt32(dt.Rows[0][0]);

            string query;
            if (count > 0)
            {
                query = $"UPDATE mcq_answers SET answer = '{answer.answer}' WHERE mcqID = {answer.mcqID} AND studentID = {answer.studentID}";
            }
            else
            {
                query = $"INSERT INTO mcq_answers (mcqID, studentID, answer) VALUES ({answer.mcqID}, {answer.studentID}, '{answer.answer}')";
            }

            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        // Save SHQ answer
        public static bool SaveShqAnswer(shqAnswerModel answer)
        {
            // Check if answer exists already and update if it does
            string checkQuery = $"SELECT COUNT(*) FROM shq_answers WHERE shqID = {answer.shqID} AND studentID = {answer.studentID}";
            DataTable dt = DatabaseHelper.Instance.GetData(checkQuery);
            int count = Convert.ToInt32(dt.Rows[0][0]);

            string query;
            if (count > 0)
            {
                query = $"UPDATE shq_answers SET answer = '{answer.answer}' WHERE shqID = {answer.shqID} AND studentID = {answer.studentID}";
            }
            else
            {
                query = $"INSERT INTO shq_answers (shqID, studentID, answer) VALUES ({answer.shqID}, {answer.studentID}, '{answer.answer}')";
            }

            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        // Check if a student has already attempted a quiz
        public static bool HasAttemptedQuiz(int studentId, int quizId)
        {
            string query = $"SELECT * FROM attempt WHERE studentID = {studentId} AND quizID = {quizId}";
            Console.WriteLine(query);
            DataTable dt = DatabaseHelper.Instance.GetData(query);
            
            if(dt==null || dt.Rows.Count==0)
            {
                return false;
            }
            else

            {
                Console.WriteLine(dt.Rows.Count);
                return true;
            }
        }

        // Mark a quiz as attempted in the quiz table
        public static bool MarkQuizAsAttempted(int quizId)
        {
            string query = $"UPDATE quiz SET attempt = true WHERE quizID = {quizId}";
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }
        
        // Calculate and save quiz results
        public static bool SaveQuizResults(resultModel result)
        {
            string query = $"INSERT INTO results (mcq_marks, shq_marks, total_marks, quizID, studentID) VALUES ({result.mcq_marks}, {result.shq_marks}, {result.total_marks}, {result.quizID}, {result.studentID})";
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        // Calculate MCQ score based on answers
        public static int CalculateMcqScore(int studentId, int quizId)
        {
            string query = @"
                SELECT COUNT(*) AS correct_count 
                FROM mcq_answers ma 
                JOIN mcqs m ON ma.mcqID = m.mcqID 
                WHERE ma.studentID = " + studentId + @" 
                AND m.quizID = " + quizId + @" 
                AND ma.answer = m.correct_opt";

            DataTable dt = DatabaseHelper.Instance.GetData(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["correct_count"]);
            }
            return 0;
        }

        // Get submitted answers for review
        public static DataTable GetMcqAnswers(int studentId, int quizId)
        {
            string query = @"
                SELECT m.mcqID, m.statement, m.option_A, m.option_B, m.option_C, m.option_D, 
                       m.correct_opt, ma.answer AS student_answer
                FROM mcqs m 
                LEFT JOIN mcq_answers ma ON m.mcqID = ma.mcqID AND ma.studentID = " + studentId + @"
                WHERE m.quizID = " + quizId;

            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable GetShqAnswers(int studentId, int quizId)
        {
            string query = @"
                SELECT sq.shqID, sq.question, sa.answer AS student_answer
                FROM short_questions sq
                LEFT JOIN shq_answers sa ON sq.shqID = sa.shqID AND sa.studentID = " + studentId + @"
                WHERE sq.quizID = " + quizId;

            return DatabaseHelper.Instance.GetData(query);
        }
    }
}
