using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WFWSupervisorsDashboard
{
    public static class AppManager
    {
        private const string DEFAULT_FILE_PATH =
            @"Data Source=C:\Users\jamie\source\repos\WFWSupervisorsDashboard\Data\WFWSupDashData.db";

        public static AppSettings Settings { get; set; } = new AppSettings
        {
            DatabaseConnectionString = DEFAULT_FILE_PATH
        };

        public static void LoadSettings()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(folderPath, "WFWSupervisorsDashBoard", "settings.json");

            if (File.Exists(filePath))
            {
                string settingsJson = File.ReadAllText(filePath);
                Settings = JsonConvert.DeserializeObject<AppSettings>(settingsJson) ?? Settings; // Load, but keep defaults if it fails 
            }
        }

        public static void SaveSettings()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(folderPath, "WFWSupervisorsDashBoard", "settings.json");

            string settingsJson = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(filePath, settingsJson);
        }
    }
}
