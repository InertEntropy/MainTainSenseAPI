using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using WFWSupervisorsDashboard.Models;
// ... other namespaces for database access (e.g., System.Data.SQLite)

namespace WFWSupervisorsDashboard.Data // Replace with your project's namespace
{
    public class DatabaseUserInfoProvider : IUserInfoProvider
    {
        private readonly SqliteConnection _connection;

        // Add a constructor to accept database connection objects
        public DatabaseUserInfoProvider(SqliteConnection connection)
        {
            _connection = connection;
        }

        public int UserID { get; private set; }
        public string DisplayName { get; private set; }
        public string Role { get; private set; }

        public void LoadUserInfo(int userId)
        {
            using (var conn = _connection) // Assuming you stored a SqliteConnection as _connection
            {
                conn.Open();

                string sql = @"SELECT  UserID, DisplayName, Role 
                       FROM Users 
                       WHERE UserID = @userId";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.UserID = reader.GetInt32("UserID"); // Access by column name
                            this.DisplayName = reader.GetString("DisplayName");
                            this.Role = reader.GetString("Role");
                        }
                        else
                        {
                            //throw new UserNotFoundException("User with ID " + userId + " not found.");
                        }
                    }
                }
            }
        }
    }
}
