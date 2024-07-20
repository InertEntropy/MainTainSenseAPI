using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MainTainSense.Data
{
    public class SQLiteDataAccess
    {
        private string _connectionString;

        public SQLiteDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<T> LoadData<T>(string sql, Func<SQLiteDataReader, T> mapper)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var results = new List<T>();
                        while (reader.Read())
                        {
                            results.Add(mapper(reader));
                        }
                        return results;
                    }
                }
            }
        }

        public void SaveData<T>(string sql, T data)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    // Add parameters based on the properties of 'data'
                    command.Parameters.AddWithValue("@param1", data.Property1);
                    // ...add more parameters as needed

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
