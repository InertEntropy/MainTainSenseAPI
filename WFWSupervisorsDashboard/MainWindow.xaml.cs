using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using System.Security.Principal;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System;         
using System.Data;   
using System.IO;

namespace WFWSupervisorsDashboard
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Member Variables / Fields 
        private string connectionString = AppManager.Settings.DatabaseConnectionString;
        private string? _displayName;
        private string username;
        private string? userRole;
        public AppSettings Settings { get; set; }

        // Properties 
        public string DisplayName
        {
            get => _displayName ?? "";
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        //Event
        public event PropertyChangedEventHandler? PropertyChanged;
        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            Settings = new AppSettings();
            AppManager.LoadSettings();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = this;
            _displayName = "";

            // Get Windows username
            username = Environment.UserName;  // Directly gets the logged-in username
            bool isAuthorized = GetUserAuthorizationStatus(username);

            if (isAuthorized)
            {
                // Load the rest of the application since the user is authorized
                // ... Your existing logic to load UI components, etc.
            }
            else
            {
                MessageBox.Show("User is not authorized to use this application.");
                Application.Current.Shutdown();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        

        private bool GetUserAuthorizationStatus(string username) // Change return type to bool
        {
            bool isAuthorized = false;
            userRole = "";
            int isEnabledValue = 0;

            using (Microsoft.Data.Sqlite.SqliteConnection conn = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT r.RoleName, u.DisplayName, u.IsEnabled 
                                         FROM Users u JOIN Roles r ON u.RoleID = r.RoleID
                                         WHERE u.Username = @username";

                SqliteCommand cmd = new SqliteCommand(query, conn);
                
                cmd.Parameters.AddWithValue("@username", username);

                SqliteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userRole = reader["RoleName"].ToString();
                    DisplayName = reader["DisplayName"].ToString();
                    isEnabledValue = reader.GetInt32("IsEnabled");
                    isAuthorized = isEnabledValue == 1;
                }
                 return isAuthorized;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(userRole);
            settingsWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
   