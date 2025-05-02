using DBHelper;
using System.Data;
using System.Data.SqlClient;

namespace Quizzy.Models.Data_Layer.quiz
{
    public class createQuizDL
    {
        public static bool deleteQuiz(string id)
        {
            try
            {
                string query = $"DELETE FROM quiz WHERE quizID = {id}";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in delete Quiz: " + ex.Message);
                return false;
            }
        }

        public static bool assignQuiz(string id)
        {
            try
            {
                string query = $"UPDATE quiz SET is_asssigned = true WHERE quizID = {id}";

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
                string query = $"UPDATE quiz SET is_asssigned = false WHERE quizID = {id}";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in unassigning Quiz: " + ex.Message);
                return false;
            }
        }
    }
}        
