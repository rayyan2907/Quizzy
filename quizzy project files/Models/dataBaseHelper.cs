using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DBHelper
{
    public class DatabaseHelper
    {
        // Azure MySQL Flexible Server connection details
        private string serverName = "test-server-quiz.mysql.database.azure.com";
        private string port = "3306";
        private string databaseName = "quizzy";
        private string databaseUser = "quizzy";
        private string databasePassword = "M.rayyan290605";

        private static DatabaseHelper _instance;
        private string connectionString;

        private DatabaseHelper()
        {
            // Azure requires SSL
            connectionString = $"server={serverName};port={port};user={databaseUser};password={databasePassword};database={databaseName};SslMode=Required;";
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

        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return dt;
        }

        public int Update(string query)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        return command.ExecuteNonQuery();
                    }
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


