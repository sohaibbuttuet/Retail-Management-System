using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace RMS
{
    internal class DatabaseHelper
    {
        private DatabaseHelper() { }

        // Modern, thread-safe Singleton implementation
        private static readonly Lazy<DatabaseHelper> _instance = new Lazy<DatabaseHelper>(() => new DatabaseHelper());
        public static DatabaseHelper Instance => _instance.Value;

        public MySqlConnection getConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // MySqlParameter array to prevent SQL injection
        public int Update(string query, MySqlParameter[] parameters = null)
        {
            using (var connection = getConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null) command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetDataTable(string query, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection connection = getConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (parameters != null) command.Parameters.AddRange(parameters);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public int ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            using (var connection = getConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null) command.Parameters.AddRange(parameters);
                    object result = command.ExecuteScalar();

                    // Handles potential DBNull returns safely
                    return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}