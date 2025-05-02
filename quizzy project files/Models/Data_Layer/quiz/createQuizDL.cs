using DBHelper;
using Quizzy.Models.DataLayer_Models;
using System.Data;
using System.Data.SqlClient;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class createQuizDL
    {
        public static bool deleteQuiz(string id)
        {
            
            
                string query = $"DELETE FROM quiz WHERE quizID = '{id}'";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            
           
        }

        public static bool assignQuiz(string id)
        {
            try
            {
                string query = $"UPDATE quiz SET is_asssign = true WHERE quizID = '{id}'";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in assigning Quiz: " + ex.Message);
                return false;
            }
        }

        public static bool unassignQuiz(string id)
        {
            try
            {
                string query = $"UPDATE quiz SET is_asssign = false WHERE quizID = '{id}'";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in unassigning Quiz: " + ex.Message);
                return false;
            }
        }


        public static bool addQuiz(quizModel q)
        {
            string query = $"insert into quiz (isPublic,quiz_name,given_time,subjectID,is_asssign,attempt) values ({q.isPublic},'{q.quizName}',{q.given_time},{q.subID},{q.isAssign},{q.attempt})";
            
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }


        public static bool updateQuiz(quizModel q)
        {
            string query = $"update quiz set  isPublic= {q.isPublic},quiz_name = '{q.quizName}',given_time={q.given_time},is_asssign={q.isAssign},attempt={q.attempt} where quizID = {q.quizID}";
            Console.WriteLine(query);
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }
    }
}        
