using System;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace WFWSupervisorsDashboard
{
    public partial class ModifyShifts : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Shift> _shifts = new ObservableCollection<Shift>();
        private string _connectionString;
        public AppSettings Settings { get; set; }
        public Shift SelectedShift { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        // Modified properties 
        public int ShiftID { get; }
        public string ShiftName { get; }
        public string StartTime { get; }
        public string EndTime { get; }
        public bool IsActive { get; }

        public ModifyShifts()
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Settings = new AppSettings();
            if (Settings != null)
            {
                _connectionString = Settings.DatabaseConnectionString;
                LoadShifts();
            }
            if (shiftsDataGrid.Items.Count > 0)
            {
                shiftsDataGrid.SelectedIndex = 0;
            }
        }

        private void LoadShifts()
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Shifts";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shiftId = reader.GetInt32(0);
                            var existingShift = _shifts.FirstOrDefault(s => s.ShiftID == shiftId);
                            if (existingShift != null)
                            {
                                // Update existing shift
                                existingShift.ShiftName = reader.GetString(1);
                                existingShift.StartTime = reader.GetString(2);
                                existingShift.EndTime = reader.GetString(3);
                                existingShift.IsActive = reader.GetInt32(4) == 1;
                            }
                            else
                            {
                                _shifts.Add(new Shift()
                                {
                                    ShiftID = reader.GetInt32(0),
                                    ShiftName = reader.GetString(1),
                                    StartTime = reader.GetString(2),
                                    EndTime = reader.GetString(3),
                                    IsActive = reader.GetInt32(4) == 1
                                });
                            }
                        }
                    }
                }
            }
            shiftsDataGrid.ItemsSource = _shifts;
        }

        private void ShiftsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedShift = shiftsDataGrid.SelectedItem as Shift;

            if (SelectedShift != null)
            {
                shiftNameTextBox.Text = SelectedShift.ShiftName;
                startTimeTextBox.Text = SelectedShift.StartTime;
                endTimeTextBox.Text = SelectedShift.EndTime;
                isActiveCheckBox.IsChecked = SelectedShift.IsActive;
            }
            else
            {
                shiftNameTextBox.Text = "";
                startTimeTextBox.Text = "";
                endTimeTextBox.Text = "";
                isActiveCheckBox.IsChecked = false;
            }
        }

        private void DeleteShiftButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedShift == null)
            {
                MessageBox.Show("Please select a shift to delete.");
                return;
            }

            // Show confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete shift '{SelectedShift.ShiftName}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var conn = new SqliteConnection(_connectionString))
                    {
                        conn.Open();

                        string sql = @"DELETE FROM Shifts WHERE ShiftID = @shiftID";

                        using (var cmd = new SqliteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@shiftID", SelectedShift.ShiftID);
                            int rowsDeleted = cmd.ExecuteNonQuery();

                            if (rowsDeleted > 0)
                            {
                                MessageBox.Show("Shift deleted successfully!");

                                // Remove the shift from the _shifts collection
                                _shifts.Remove(SelectedShift);

                                // Optionally: Refresh the DataGrid
                                // shiftsDataGrid.ItemsSource = null; 
                                // shiftsDataGrid.ItemsSource = _shifts;
                            }
                            else
                            {
                                MessageBox.Show("Error deleting shift.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting shift: " + ex.Message);
                }
            }
        }


        private void AddNewShiftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"INSERT INTO Shifts 
                              (ShiftName, StartTime, EndTime, IsActive) 
                           VALUES 
                              (@shiftName, @startTime, @endTime, @isActive)";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@shiftName", shiftNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@startTime", startTimeTextBox.Text);
                        cmd.Parameters.AddWithValue("@endTime", endTimeTextBox.Text);
                        cmd.Parameters.AddWithValue("@isActive", isActiveCheckBox.IsChecked.HasValue ? (isActiveCheckBox.IsChecked.Value ? 1 : 0) : 0);

                        int rowsInserted = cmd.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("New Shift added successfully!");

                            // Clear the input fields
                            shiftNameTextBox.Text = "";
                            startTimeTextBox.Text = "";
                            endTimeTextBox.Text = "";
                            isActiveCheckBox.IsChecked = false;
                            LoadShifts(); 
                        }
                        else
                        {
                            MessageBox.Show("Error adding shift.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding shift: " + ex.Message);
            }
        }


        private void UpdateShiftButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedShift == null)
            {
                MessageBox.Show("Please select a shift to update.");
                return;
            }

            try
            {
                using (var conn = new SqliteConnection(_connectionString))
                {
                    conn.Open();

                    string sql = @"UPDATE Shifts SET 
                            ShiftName = @shiftName, 
                            StartTime = @startTime, 
                            EndTime = @endTime, 
                            IsActive = @isActive
                        WHERE ShiftID = @shiftID";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@shiftName", shiftNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@startTime", startTimeTextBox.Text);
                        cmd.Parameters.AddWithValue("@endTime", endTimeTextBox.Text);
                        cmd.Parameters.AddWithValue("@isActive", isActiveCheckBox.IsChecked.HasValue ? (isActiveCheckBox.IsChecked.Value ? 1 : 0) : 0);
                        cmd.Parameters.AddWithValue("@shiftID", SelectedShift.ShiftID);

                        int rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            MessageBox.Show("Shift updated successfully!");

                            // Update the in-memory _shifts collection
                            SelectedShift.ShiftName = shiftNameTextBox.Text;
                            SelectedShift.StartTime = startTimeTextBox.Text;
                            SelectedShift.EndTime = endTimeTextBox.Text;
                            SelectedShift.IsActive = isActiveCheckBox.IsChecked.Value;

                            // Refresh the DataGrid if needed
                            // shiftsDataGrid.ItemsSource = null;
                            // shiftsDataGrid.ItemsSource = _shifts;                  
                        }
                        else
                        {
                            MessageBox.Show("Error updating shift.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating shift: " + ex.Message);
            }
        }
    }
}

