using DBHelper;
using Quizzy.Models.Buisness_Models;
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

        public static bool addMcq(mcqModel mcq)
        {
            string query = $"insert into mcqs (option_A,option_B,option_C,option_D,correct_opt,quizID,statement) values ('{mcq.opt1}','{mcq.opt2}','{mcq.opt3}','{mcq.opt4}','{mcq.corr_opt}',{mcq.quizID},'{mcq.description}')";
            Console.WriteLine(query );
            int row= DatabaseHelper.Instance.Update(query);
            return row > 0;


        }

        public static bool addShq(shortQuestionModel shq)
        {
            string query = $"insert into short_questions (question,quizID) values ('{shq.shortQuestion}',{shq.quizID})";
            Console.WriteLine(query );
            int rows= DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        public static quiz_model getQuizObj(string id)
        {
            Console.WriteLine("we are getting the object of quiz  with id  "+id);
            string query = $"select quizID,isPublic,quiz_name,given_time,subjectID,is_asssign,attempt from quiz where quizID={id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                quiz_model q = new quiz_model();
                q.quizID= id;
                q.isPublic = Convert.ToBoolean(dt.Rows[0]["isPublic"]);
                q.isAssign = Convert.ToBoolean(dt.Rows[0]["is_asssign"]);
                q.attempt = Convert.ToBoolean(dt.Rows[0]["attempt"]);
                q.quizName = dt.Rows[0]["quiz_name"].ToString();
                q.given_time = dt.Rows[0]["given_time"].ToString();
                q.subID = dt.Rows[0]["subjectID"].ToString();



                return q;
            }

            return null;
        }

        public static DataTable getMcqs(quiz_model q)
        {
            string query = $"select mcqID,statement,option_A,option_B,option_C,option_D,correct_opt from mcqs m join quiz q on q.quizID=m.quizID where q.subjectID={q.subID} and m.quizID ={q.quizID}";
            return DatabaseHelper.Instance.GetData(query);
        }

        public static DataTable getShqs(quiz_model q)
        {
            string query = $"select shqID,question,quizID from short_questions  where quizID={q.quizID} ";
            Console.WriteLine(query);
            return DatabaseHelper.Instance.GetData(query);
        }


        public static bool deleteMcq(string id)
        {
            string query = $"delete from mcqs where mcqID={id}";
            Console.WriteLine(query);
            int rows= DatabaseHelper.Instance.Update(query);
            return rows>0 ;
        }

        public static bool deleteShq(string id)
        {
            string query = $"delete from short_questions where shqID={id}";
            Console.WriteLine(query);
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

        public static mcq_model getmcqObj(string id)
        {
            string query = $"select option_A,option_B,option_C,option_D,correct_opt,quizID,statement from mcqs where mcqID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query) ;

            mcq_model m = new mcq_model() ;

            m.mcq_id = id ;
            m.opt1 = dt.Rows[0]["option_A"].ToString();
            m.opt2 = dt.Rows[0]["option_B"].ToString();
            m.opt3 = dt.Rows[0]["option_C"].ToString();
            m.opt4 = dt.Rows[0]["option_D"].ToString();
            m.corr_opt = dt.Rows[0]["correct_opt"].ToString();
            m.description = dt.Rows[0]["statement"].ToString();
            Console.WriteLine("mcq id in dl to be update is " + m.mcq_id);
            return m;

        }

        public static bool updateMcq(mcq_model mcq)
        {
            string query = $"update mcqs set option_A = '{mcq.opt1}' , option_B='{mcq.opt2}',option_C='{mcq.opt3}',option_D='{mcq.opt4}',correct_opt='{mcq.corr_opt}',statement='{mcq.description}' where mcqID={mcq.mcq_id}";
            Console.WriteLine (query);
            int rows =  DatabaseHelper.Instance.Update(query) ;
            return rows > 0;
        }
        public static shq_model getshqObj(string id)
        {
            string query = $"select shqID,question,quizID from short_questions  where shqID = {id}";
            DataTable dt = DatabaseHelper.Instance.GetData(query);

            shq_model s = new shq_model() ; 

            s.shqID = id ;
            Console.WriteLine (s.shqID,id);
            Console.WriteLine(query);
            s.shortQuestion = dt.Rows[0]["question"].ToString();
            s.shqID = id;
            Console.WriteLine("shq id assigne " + s.shqID);
         

            return s;

        }


        public static bool updateSHQ(shq_model s)
        {
            string query = $"update short_questions set question = '{s.shortQuestion}' where shqID =  {s.shqID}";
            Console.WriteLine(query);
            int rows = DatabaseHelper.Instance.Update(query);
            return rows > 0;
        }

    }
}        
