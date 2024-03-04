using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;
using Microsoft.Data.Sqlite; // If you haven't already 

namespace WFWSupervisorsDashboard
{
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {

        public AppSettings Settings { get; } = new AppSettings();
        private string _userRole;

        public SettingsWindow(string userRole) // Pass userRole when creating
        {
            InitializeComponent();
            _userRole = userRole;
            DataContext = this;
            AppManager.LoadSettings();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Show admin settings if the role is Admin
            if (_userRole == "Admin")
            {
                adminSettingsPanel.Visibility = Visibility.Visible;
                supervisorSettingsPanel.Visibility = Visibility.Visible; // Show for admins
            }
            else if (_userRole == "Supervisor")
            {
                supervisorSettingsPanel.Visibility = Visibility.Visible; // Show for supervisors
            }
        }

        private void ModifyEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            var modifyEmployees = new ModifyEmployees(); 
            modifyEmployees.Show();
        }
        private void ModifyMachinesButton_Click(object sender, RoutedEventArgs e)
        {
            var modifyMachines = new ModifyMachines(); 
            modifyMachines.Show();
        }
        private void ModifyShiftsButton_Click(object sender, RoutedEventArgs e)
        {
            var modifyShifts = new ModifyShifts();
            modifyShifts.Settings = Settings;  // Pass the settings here
            modifyShifts.Show();
        }
        private void FutureButton1_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FutureButton2_Click(object sender, RoutedEventArgs e)
        {

        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            AppManager.SaveSettings(); // Call the static save method
        }
        private void LoadSettings()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = System.IO.Path.Combine(folderPath, "YourAppName", "settings.json");

            if (File.Exists(filePath))
            {
                string settingsJson = File.ReadAllText(filePath);
                var loadedSettings = JsonConvert.DeserializeObject<AppSettings>(settingsJson); // Deserialize

                Settings.DatabaseConnectionString = loadedSettings.DatabaseConnectionString; // Update values
                                                                                             // ... update other properties similarly ... 
            }
            else
            {
                // Optional: Use default settings if the file doesn't exist
            }
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQLite Databases (*.db;*.sqlite)|*.db;*.sqlite"; // Filter for database files

            if (openFileDialog.ShowDialog() == true)
            {
                Settings.DatabaseConnectionString = openFileDialog.FileName; // Update the setting 
            }
        }
    }
}
