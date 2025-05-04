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
        private string databaseName = "quizzy"; // Your actual DB name
        private string databaseUser = "quizzy"; // Must include server name
        private string databasePassword = "M.rayyan290605"; // Your actual password

        private static DatabaseHelper _instance;
        private MySqlConnection connection;

        private DatabaseHelper()
        {
            // Azure requires SSL
            string connectionString = $"server={serverName};port={port};user={databaseUser};password={databasePassword};database={databaseName};SslMode=Required;";
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
