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
using Microsoft.Extensions.DependencyInjection;
using WFWSupervisorsDashboard.Models;

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
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserInfoProvider _userInfoProvider;
        private int userId;

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
        public MainWindow(IServiceProvider serviceProvider, IUserInfoProvider userInfoProvider)
        {
            InitializeComponent();
            Settings = new AppSettings();
            AppManager.LoadSettings();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = this;
            _displayName = "";
            _serviceProvider = serviceProvider;
            _userInfoProvider = userInfoProvider;

            // Get Windows username
            username = Environment.UserName;  // Directly gets the logged-in username
            bool isAuthorized = GetUserAuthorizationStatus(username);

            if (isAuthorized)
            {
                // Assuming you have your DI setup from earlier examples
                //IUserInfoProvider _userInfoProvider = (IUserInfoProvider)(Application.Current as App).ServiceProvider.GetService(typeof(IUserInfoProvider));

                // Create MainViewModel instance
                MainViewModel mainViewModel = new MainViewModel(userInfoProvider, userId);

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
                    userId = reader.GetInt32("UserID");
                    //Application.Current.Properties["CurrentUserID"] = userId;
                    //Application.Current.Properties["CurrentUserRole"] = userRole;
                   // Application.Current.Properties["CurrentDisplayName"] = DisplayName;
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
   