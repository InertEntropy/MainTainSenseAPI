using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Threading;
using NLog;

namespace MainTainSense.Data
{
    public class UserManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        public UserManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DateTimeAndUser GetCurrentDateTimeAndUser()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var currentUser = WindowsIdentity.GetCurrent().Name;
            var username = currentUser.Substring(currentUser.IndexOf('\\') + 1);
            return new DateTimeAndUser { DateTime = now, User = username };
        }

        public void HandleDatabaseError(Action databaseAction, string methodName)
        {
            try
            {
                databaseAction();
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex, "Failed to add user.");
                HandleSqliteException(ex, methodName);
            }
        }

        private void HandleSqliteException(SQLiteException sqliteEx, string methodName)
        {
            const int maxRetries = 3; // Adjust as needed
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                switch (sqliteEx.ErrorCode)
                {
                    case 19: // Constraint violation
                        throw new DatabaseConstraintViolationException("Cannot create: item already exists.");

                    case 1: // Database locked
                        int delay = 500; // Initial delay in milliseconds (adjust as needed)
                        logger.Warn("Database locked in {methodName}. Retrying after {delay}ms", methodName, delay);
                        Thread.Sleep(delay);
                        retryCount++;
                        break;

                    case 6:
                        logger.Error(sqliteEx, "Failed to open database. Database error in {methodName}", methodName);
                        throw new DatabaseOperationException("Failed to open database.", methodName, sqliteEx);

                    case 11:
                        string message = "Critical Database Error: Database corruption detected. Data might be compromised. Contact support immediately.";
                        logger.Fatal(sqliteEx, "Database corruption in {methodName}. Details: {details}", methodName, sqliteEx.ToString()); // Log extensively
                        throw new DatabaseOperationException(message, methodName, sqliteEx);

                    default:
                        logger.Error(sqliteEx, "An unhandled database error occurred. Database error in {methodName}", methodName);
                        throw new DatabaseOperationException("An unhandled database error occurred.", methodName, sqliteEx);
                }
            }
            logger.Error(sqliteEx, "Database error in {methodName} after retries", methodName);
            throw new DatabaseOperationException("Failed to execute after retries. Database may be unavailable.", methodName, sqliteEx);
        }

        public void AddUser(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                HandleDatabaseError(() =>
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"INSERT INTO Users (
                    FirstName, LastName, JobTitleId, DepartmentId, IsActive, LastUpdated, UpdatedBy,
                    UserName, Email, NormalizedUserName, NormalizedEmail
                ) VALUES (
                    @firstName, @lastName, @jobTitleId, @departmentId, @isActive, @lastUpdated, @updatedBy, 
                    @userName, @email, @normalizedUserName, @normalizedEmail 
                )";

                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@jobTitleId", user.JobTitle.Id);
                        command.Parameters.AddWithValue("@departmentId", user.Department.Id);
                        command.Parameters.AddWithValue("@isActive", user.IsActive);
                        string lastUpdated = GetCurrentDateTimeAndUser().DateTime;
                        string updatedBy = GetCurrentDateTimeAndUser().User;
                        command.Parameters.AddWithValue("@lastUpdated", lastUpdated);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@userName", user.UserName);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@normalizedUserName", user.NormalizedUserName);
                        command.Parameters.AddWithValue("@normalizedEmail", user.NormalizedEmail);

                        command.ExecuteNonQuery();
                        logger.Info($"User created: {user.FirstName} {user.LastName} (username: {user.UserName})");
                    }
                }, "AddUser");
            }
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                HandleDatabaseError(() =>
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "SELECT * FROM Users";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapReaderToUser(reader));
                            }
                        }
                    }
                }, "GetUsers");
            }

            return users;
        }

        private User MapReaderToUser(SQLiteDataReader reader)
        {
            try
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    JobTitleId = reader.GetInt32(reader.GetOrdinal("JobTitleId")),
                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) != 0 ? 1 : 0,
                    LastUpdated = reader.GetString(reader.GetOrdinal("LastUpdated")),
                    UpdatedBy = reader.GetString(reader.GetOrdinal("UpdatedBy")),
                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    NormalizedUserName = reader.GetString(reader.GetOrdinal("NormalizedUserName")),
                    NormalizedEmail = reader.GetString(reader.GetOrdinal("NormalizedEmail")),

                    JobTitle = GetJobTitleById(reader.GetInt32(reader.GetOrdinal("JobTitleId"))),
                    Department = GetDepartmentById(reader.GetInt32(reader.GetOrdinal("DepartmentId")))
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error mapping user from database reader.");
                throw;
            }
        }

        private UserJobTitles GetJobTitleById(int jobTitleId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open(); // Open the connection
                using (var command = new SQLiteCommand(conn))
                {
                    command.CommandText = "SELECT * FROM UserJobTitles WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", jobTitleId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // If a matching job title is found
                        {
                            return MapReaderToJobTitle(reader); // Create a UserJobTitles object
                        }
                        else
                        {
                            return null; // Or handle the case where no job title is found
                        }
                    }
                }
            }
        }

        private UserJobTitles MapReaderToJobTitle(SQLiteDataReader reader)
        {
            return new UserJobTitles
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
            };
        }
        private Departments GetDepartmentById(int departmentId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open(); // Open the connection
                using (var command = new SQLiteCommand(conn))
                {
                    command.CommandText = "SELECT * FROM Departments WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", departmentId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // If a matching job title is found
                        {
                            return MapReaderToDepartment(reader); 
                        }
                        else
                        {
                            return null; 
                        }
                    }
                }
            }
        }

        private Departments MapReaderToDepartment(SQLiteDataReader reader)
        {
            return new Departments
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                // ... (Map other properties of UserJobTitles) ...
            };
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                HandleDatabaseError(() =>
                {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                    {

                        command.CommandText = @"UPDATE Users SET 
                                        FirstName = @firstName,
                                        LastName = @lastName,
                                        JobTitleId = @jobTitleId,
                                        DepartmentId = @departmentId,
                                        IsActive = @isActive,
                                        UserName = @userName,
                                        Email = @email,
                                        NormalizedUserName = @normalizedUserName,
                                        NormalizedEmail = @normalizedEmail,
                                        LastUpdated = @lastUpdated,
                                        UpdatedBy = @updatedBy
                                        WHERE Id = @id";

                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@jobTitleId", user.JobTitleId);
                        command.Parameters.AddWithValue("@departmentId", user.DepartmentId);
                        command.Parameters.AddWithValue("@isActive", user.IsActive);
                        command.Parameters.AddWithValue("@userName", user.UserName);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@normalizedUserName", user.NormalizedUserName);
                        command.Parameters.AddWithValue("@normalizedEmail", user.NormalizedEmail    );
                        command.Parameters.AddWithValue("@lastUpdated", GetCurrentDateTimeAndUser().DateTime);
                        command.Parameters.AddWithValue("@updatedBy", GetCurrentDateTimeAndUser().User);
                        command.Parameters.AddWithValue("@id", user.Id);

                        command.ExecuteNonQuery();
                    }
                }, "UpdateUser");
            }
        }
    }

    [Serializable]
    public class DatabaseOperationException : Exception
    {
        public string SqlStatement { get; }
        public DateTime Timestamp { get; }

        public DatabaseOperationException(string message, string sqlStatement) : base(message)
        {
            SqlStatement = sqlStatement;
            Timestamp = DateTime.Now;
        }

        protected DatabaseOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SqlStatement = info.GetString("SqlStatement");
            Timestamp = info.GetDateTime("Timestamp");
        }

        public DatabaseOperationException(string message, string sqlStatement, Exception innerException)
        : base(message, innerException)
        {
            SqlStatement = sqlStatement;
            Timestamp = DateTime.Now;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SqlStatement", SqlStatement);
            info.AddValue("Timestamp", Timestamp);
        }
    }

    [Serializable]
    public class DatabaseConstraintViolationException : DatabaseOperationException
    {
        public string ConstraintName { get; }
        public string TableName { get; }
        public object ConflictingValue { get; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DatabaseConstraintViolationException(string message)
            : base(message, null)
        {
            
            logger.Error(this, "Database constraint violation occurred. Message: {message}",
                message);
        }

        protected DatabaseConstraintViolationException(SerializationInfo info, StreamingContext context)
            : base(info.GetString("message"), null) // Get the message from SerializationInfo 
        {
            ConstraintName = info.GetString("ConstraintName");
            TableName = info.GetString("TableName");
            ConflictingValue = info.GetValue("ConflictingValue", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ConstraintName", ConstraintName);
            info.AddValue("TableName", TableName);
            info.AddValue("ConflictingValue", ConflictingValue);
        }
    }
}

