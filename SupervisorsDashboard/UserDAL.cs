using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;


namespace SupervisorsDashboard
{
    public class UserDal
    {
        private readonly string appDataPath;
        private readonly string connectionString;
        public enum UserRole { Admin, Supervisor, LeadOperator, Other, Unauthorized }

        public UserDal() // Constructor
        {
            appDataPath = Path.Combine(Application.StartupPath, "Data");
           
            connectionString = $@"Data Source={Path.Combine(appDataPath, "users.db")};Version=3;";
            MessageBox.Show(connectionString);
        }
        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Password FROM Users WHERE Username = @username";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    string storedPasswordHash = cmd.ExecuteScalar() as string;

                    // Replace placeholders with actual password hashing logic
                    if (storedPasswordHash == null)
                    {
                        return false; // User not found
                    }
                    string enteredPasswordHash = HashPassword(password);
                    return storedPasswordHash == enteredPasswordHash;
                }
            }
            catch (SQLiteException ex)
            {
                // Handle the error
                LogErrorToFile("Database Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please contact your administrator.");
                return false;
            }

        }
        // Placeholder - Implement a secure password hashing method
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool IsUserAdmin()
        {
            // Using WindowsIdentity and WindowsPrincipal for authentication
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole("SupervisorsDashboardAdmins"); // Replace with your admin group name
            }
        }

        public void AddUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@username, @password, @isAdmin)";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@isAdmin", 0); // Assuming regular users start with IsAdmin = 0
                cmd.ExecuteNonQuery();
            }
        }

        public void LogErrorToFile(string message)
        {
            string logFilePath = Path.Combine(appDataPath, "error.log"); // Adjust if needed
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        public void CreateDefaultAdmin()
        {
            string query = "INSERT INTO Users (Username, Password, IsAdmin, IsDefaultAdmin) VALUES ('defaultadmin', @password, 1, 1);";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                // Hash the default admin password
                string hashedPassword = HashPassword("P@ssw0rd!");
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                cmd.ExecuteNonQuery(); // Execute the INSERT query
            }
        }
        public UserRole GetUserRole(string username)
        {
            string query = @"SELECT r.RoleName 
                     FROM Users u JOIN Roles r ON u.RoleID = r.RoleID 
                     WHERE u.UserName = @username";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                string roleName = cmd.ExecuteScalar() as string;

                // Map roleName to UserRole enum
                switch (roleName)
                {
                    case "Admin": return UserRole.Admin;
                    case "Supervisor": return UserRole.Supervisor;
                    case "Lead Operator": return UserRole.LeadOperator;
                    case "Other": return UserRole.Other;
                    default: return UserRole.Unauthorized;
                }
            }
        }
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT u.UserID, u.UserName, u.IsAdmin, u.IsDefaultAdmin, 
                                u.IsEnabled, r.RoleName 
                         FROM Users u JOIN Roles r ON u.RolesId = r.RoleId";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User();
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.UserName = reader["UserName"].ToString();
                    user.IsAdmin = Convert.ToBoolean(reader["ISAdmin"]);
                    user.IsDefaultAdmin = Convert.ToBoolean(reader["IsDefaultAdmin"]);
                    user.IsEnabled = Convert.ToBoolean(reader["IsEnabled"]);
                    user.Role = (UserRole)Enum.Parse(typeof(UserRole), reader["RoleName"].ToString());
                }
            }
            return users;
        }
    }
}

