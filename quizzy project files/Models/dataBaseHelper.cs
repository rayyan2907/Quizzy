//using MySql.Data.MySqlClient;
//using System;
//using System.Data;

//namespace DBHelper
//{
//    public class DatabaseHelper
//    {
//        // Azure MySQL Flexible Server connection details
//        private string serverName = "test-server-quiz.mysql.database.azure.com";
//        private string port = "3306";
//        private string databaseName = "quizzy";
//        private string databaseUser = "quizzy";
//        private string databasePassword = "M.rayyan290605";

//        private static DatabaseHelper _instance;
//        private string connectionString;

//        private DatabaseHelper()
//        {
//            // Azure requires SSL
//            connectionString = $"server={serverName};port={port};user={databaseUser};password={databasePassword};database={databaseName};SslMode=Required;";
//        }

//        public static DatabaseHelper Instance
//        {
//            get
//            {
//                if (_instance == null)
//                    _instance = new DatabaseHelper();
//                return _instance;
//            }
//        }

//        private MySqlConnection CreateConnection()
//        {
//            return new MySqlConnection(connectionString);
//        }

//        public DataTable GetData(string query)
//        {
//            DataTable dt = new DataTable();
//            try
//            {
//                using (var conn = CreateConnection())
//                {
//                    conn.Open();
//                    using (var command = new MySqlCommand(query, conn))
//                    {
//                        using (var reader = command.ExecuteReader())
//                        {
//                            dt.Load(reader);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error: " + ex.Message);
//            }
//            return dt;
//        }

//        public int Update(string query)
//        {
//            try
//            {
//                using (var conn = CreateConnection())
//                {
//                    conn.Open();
//                    using (var command = new MySqlCommand(query, conn))
//                    {
//                        return command.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error: " + ex.Message);
//                return -1;
//            }
//        }
//    }
//}




using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DBHelper
{
    public class DatabaseHelper
    {
        private String serverName = "127.0.0.1";
        private String port = "3306";
        private String databaseName = "quizzy";
        private String databaseUser = "root";
        private String databasePassword = "anas123@";

        private static DatabaseHelper _instance;
        private MySqlConnection connection;

        private DatabaseHelper()
        {
            string connectionString = $"server={serverName};port={port};user={databaseUser};database={databaseName};password={databasePassword};SslMode=None;";
            connection = new MySqlConnection(connectionString);
        }

        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseHelper();
                return _instance;
            }
        }

        public MySqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return connection;
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var command = new MySqlCommand(query, GetConnection()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        public int Update(string query)
        {
            try
            {
                using (var command = new MySqlCommand(query, GetConnection()))
                {
                    int result = command.ExecuteNonQuery();
                    CloseConnection();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }
    }
}

