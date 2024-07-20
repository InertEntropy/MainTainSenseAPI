using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace MainTainSense.Data
{
    public class DataRepository
    {
        public readonly string _connectionString = "Data Source=C:\\Users\\jamie\\source\\repos\\MainTainSense\\Data\\MainTainSense.db";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger(); 
        private readonly string _currentUser;
        private readonly string _currentTime;

        public DataRepository()
        {
            _currentUser = WindowsIdentity.GetCurrent().Name;
            _currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public bool IsAuthorizedUser(string username)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM AuthorizedUsers WHERE UserName = @userName";
                    command.Parameters.AddWithValue("@userName", username);

                    int userCount = (int)(long)command.ExecuteScalar();
                    return userCount > 0;
                }
            }
        }

        private void HandleDatabaseError(Action databaseAction, string operationName = null)
        {
            try
            {
                databaseAction();
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex, "SQLite Error in {operation}", operationName ?? "database operation");
                switch (ex.ErrorCode)
                {
                    case (int)SQLiteErrorCode.Constraint:
                        MessageBox.Show("Cannot create. Item may already exist", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case (int)SQLiteErrorCode.Busy:
                        MessageBox.Show("Database is temporarily busy, please try again.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case (int)SQLiteErrorCode.CantOpen:
                        MessageBox.Show("Failed to open database.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case (int)SQLiteErrorCode.Corrupt:
                        MessageBox.Show("Database corruption detected. Application terminating.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    default:
                        MessageBox.Show("An unexpected database error occurred. Please contact support.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Database Error in {operation}", operationName ?? "database operation");
                MessageBox.Show("A database error occurred. Please contact support.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveUserPreferences(string UserName, UserPreferences preferences)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"INSERT OR REPLACE INTO UserPreferences (
                                        UserName, PrimaryColor, SecondaryColor, BackgroundColor, TextColor, DefaultFontName, DefaultFontSize, DefaultFontStyle, HeadingFontName, HeadingFontSize, HeadingFontStyle)
                                    VALUES (
                                        @userName, @primaryColor, @secondaryColor, @backgroundColor, @textColor, @defaultFontName, @defaultFontSize, @defaultFontStyle, @headingFontName, @headingFontSize, @headingFontStyle)";

                    command.Parameters.AddWithValue("@userName", UserName);
                    command.Parameters.AddWithValue("@primaryColor", preferences.PrimaryColor);
                    command.Parameters.AddWithValue("@secondaryColor", preferences.SecondaryColor);
                    command.Parameters.AddWithValue("@backgroundColor", preferences.BackgroundColor);
                    command.Parameters.AddWithValue("@textColor", preferences.TextColor);
                    command.Parameters.AddWithValue("@defaultFontName", preferences.DefaultFontName);
                    command.Parameters.AddWithValue("@defaultFontSize", preferences.DefaultFontSize);
                    command.Parameters.AddWithValue("@defaultFontStyle", (int)preferences.DefaultFontStyle);
                    command.Parameters.AddWithValue("@headingFontName", preferences.HeadingFontName);
                    command.Parameters.AddWithValue("@headingFontSize", preferences.HeadingFontSize);
                    command.Parameters.AddWithValue("@headingFontStyle", (int)preferences.HeadingFontStyle);
                    command.ExecuteNonQuery();
                }
            }
        }

        public UserPreferences LoadUserPreferences(string UserName)
        {
            var preferences = new UserPreferences();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM UserPreferences WHERE UserName = @userName";
                    command.Parameters.AddWithValue("@userName", UserName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            preferences.PrimaryColor = reader["PrimaryColor"].ToString();
                            preferences.SecondaryColor = reader["SecondaryColor"].ToString();
                            preferences.BackgroundColor = reader["BackgroundColor"].ToString();
                            preferences.TextColor = reader["TextColor"].ToString();
                            preferences.DefaultFontName = reader["DefaultFontName"].ToString();
                            preferences.DefaultFontSize = float.Parse(reader["DefaultFontSize"].ToString());
                            preferences.DefaultFontStyle = (FontStyle)int.Parse(reader["DefaultFontStyle"].ToString());
                            preferences.HeadingFontName = reader["HeadingFontName"].ToString();
                            preferences.HeadingFontSize = float.Parse(reader["HeadingFontSize"].ToString());
                            preferences.HeadingFontStyle = (FontStyle)int.Parse(reader["HeadingFontStyle"].ToString());

                        }
                        else
                        {
                            // Set defaults
                            preferences.PrimaryColor = "#2A55F4";   // Use hex format
                            preferences.SecondaryColor = "#90CAF9"; // Use hex format
                            preferences.BackgroundColor = "#000000"; // Use hex format
                            preferences.TextColor = "#FFFFFF";      // Use hex format
                            preferences.DefaultFontName = "Arial";
                            preferences.DefaultFontSize = 10.0f;
                            preferences.DefaultFontStyle = FontStyle.Regular;
                            preferences.HeadingFontName = "Arial";
                            preferences.HeadingFontSize = 12.0f;
                            preferences.HeadingFontStyle = FontStyle.Bold;
                            SaveUserPreferences(UserName, preferences);
                        }
                    }
                }
            }
            return preferences;
        }

        public void AddAsset(Assets asset)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {

                try
                {
                    HandleDatabaseError(() =>
                    {
                        connection.Open();
                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = @"INSERT INTO Assets 
                                    (AssetName, AssetTypeId, AssetDescription, LocationId, StatusId, Manufacturer, SerialNumber,
                                     InstallDate, VisibleProduction, IsActive, LastUpdated, UpdatedBy) 
                                    VALUES 
                                    (@assetName, @assetTypeId, @assetDescription, @locationId, @statusId, @manufacturer, @serialNumber,
                                     @installDate, @visibleProduction, @isActive, @lastUpdated, @updatedBy)";

                            command.Parameters.AddWithValue("@assetName", asset.AssetName);
                            command.Parameters.AddWithValue("@assetTypeId", asset.AssetTypeId);
                            command.Parameters.AddWithValue("@assetDecription", asset.AssetDescription);
                            command.Parameters.AddWithValue("@locationId", asset.LocationId);
                            command.Parameters.AddWithValue("@statusId", asset.StatusId);
                            command.Parameters.AddWithValue("@manufacturer", asset.Manufacturer);
                            command.Parameters.AddWithValue("@serialNumber", asset.SerialNumber);
                            command.Parameters.AddWithValue("@installDate", asset.InstallDate);
                            command.Parameters.AddWithValue("@visibleProduction", asset.VisibleProduction);
                            command.Parameters.AddWithValue("@isActive", asset.IsActive);
                            command.Parameters.AddWithValue("@lastUpdated", _currentTime);
                            command.Parameters.AddWithValue("@updatedBy", _currentUser);

                            command.ExecuteNonQuery();
                        } 
                    }, "AddAsset");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Database Error in Add Asset");
                    MessageBox.Show("A database error occurred. Please contact support.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        public List<Assets> GetAssets()
        {
            var assets = new List<Assets>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "SELECT * FROM Assets";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var asset = new Assets
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    AssetName = reader["AssetName"].ToString(),
                                    AssetDescription = reader["AssetDescription"].ToString(),
                                    AssetTypeId = Convert.ToInt32(reader["AssetTypeId"]),
                                    LocationId = Convert.ToInt32(reader["LocationId"]),
                                    StatusId = Convert.ToInt32(reader["StatusId"]),
                                    Manufacturer = reader["Manufacturer"].ToString(),
                                    ModelNumber = reader["ModelNumber"].ToString(),
                                    SerialNumber = reader["SerialNumber"].ToString(),
                                    InstallDate = reader["InstallDate"].ToString(),
                                    VisibleProduction = Convert.ToInt32(reader["VisibleProduction"]),
                                    IsActive = Convert.ToInt32(reader["IsActive"]),
                                    LastUpdated = reader["LastUpdated"].ToString(),
                                    UpdatedBy = reader["UpdatedBy"].ToString()
                                };
                                assets.Add(asset);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex, "SQLite Error in GetAssets: {errorCode} - {message}", ex.ErrorCode, ex.Message);
                return assets;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Database Error in GetAssets: {message}", ex.Message);
                return assets;
            }
            return assets;
        }

        public List<Assets> GetAssetsByFilters(int? assetTypeId = null, int? locationId = null, int? statusId = null, string updatedBy = null)
        {
            var assets = new List<Assets>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    string query = "SELECT * FROM Assets WHERE 1 = 1";

                    if (assetTypeId.HasValue)
                    {
                        query += " AND AssetTypeId = @assetTypeId";
                        command.Parameters.AddWithValue("@assetTypeId", assetTypeId.Value);
                    }

                    if (locationId.HasValue)
                    {
                        query += " AND LocationId = @locationId";
                        command.Parameters.AddWithValue("@locationId", locationId.Value);
                    }

                    if (statusId.HasValue)
                    {
                        query += " AND StatusId = @statusId";
                        command.Parameters.AddWithValue("@statusId", statusId.Value);
                    }

                    if (!string.IsNullOrEmpty(updatedBy))
                    {
                        query += " AND UpdatedBy = @updatedBy";
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    }
 
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var asset = new Assets
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                AssetName = reader["AssetName"].ToString(),
                                AssetDescription = reader["AssetDescription"].ToString(),
                                AssetTypeId = Convert.ToInt32(reader["AssetTypeId"]),
                                LocationId = Convert.ToInt32(reader["LocationId"]),
                                StatusId = Convert.ToInt32(reader["StatusId"]),
                                Manufacturer = reader["Manufacturer"].ToString(),
                                ModelNumber = reader["ModelNumber"].ToString(),
                                SerialNumber = reader["SerialNumber"].ToString(),
                                InstallDate = reader["InstallDate"].ToString(),
                                VisibleProduction = Convert.ToInt32(reader["VisibleProduction"]),
                                IsActive = Convert.ToInt32(reader["IsActive"]),
                                LastUpdated = reader["LastUpdated"].ToString(),
                                UpdatedBy = reader["UpdatedBy"].ToString()
                            };
                            assets.Add(asset);
                        }
                    }
                }
            }
            return assets;
        }

        public void UpdateAsset(Assets asset)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"UPDATE Assets SET 
                                        AssetName = @name,
                                        AssetTypeId = @assetTypeId,
                                        AssetDescription = @description,
                                        LocationId = @locationId,
                                        StatusId = @statusId,
                                        Manufacturer = @manufacturer,
                                        ModelNumber = @modelNumber,
                                        SerialNumber = @serialNumber,
                                        InstallDate = @installdate,
                                        VisibleProduction = @visibleProduction,
                                        IsActive = @isActive,
                                        LastUpdated = @lastUpdated,
                                        UpdatedBy = @updatedBy
                                    WHERE Id = @id";

                    command.Parameters.AddWithValue("@assetName", asset.AssetName);
                    command.Parameters.AddWithValue("@assetTypeId", asset.AssetTypeId);
                    command.Parameters.AddWithValue("@assetDecription", asset.AssetDescription);
                    command.Parameters.AddWithValue("@locationId", asset.LocationId);
                    command.Parameters.AddWithValue("@statusId", asset.StatusId);
                    command.Parameters.AddWithValue("@manufacturer", asset.Manufacturer);
                    command.Parameters.AddWithValue("@serialNumber", asset.SerialNumber);
                    command.Parameters.AddWithValue("@installDate", asset.InstallDate);
                    command.Parameters.AddWithValue("@visibleProduction", asset.VisibleProduction);
                    command.Parameters.AddWithValue("@isActive", asset.IsActive);
                    command.Parameters.AddWithValue("@lastUpdated", _currentTime);
                    command.Parameters.AddWithValue("@updatedBy", _currentUser);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
