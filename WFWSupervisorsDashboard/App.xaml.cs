using System.Configuration;
using System.Windows;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using WFWSupervisorsDashboard.Data;
using WFWSupervisorsDashboard.Models;
using WFWSupervisorsDashboard;

namespace WFWSupervisorsDashboard
{
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load settings
            AppManager.LoadSettings();

            // Configure Services
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Create MainWindow
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Fetch connection string from settings 
            var connectionString = AppManager.Settings.DatabaseConnectionString;

            // Register dependencies
            services.AddSingleton(new SqliteConnection(connectionString));
            services.AddSingleton<IUserInfoProvider, DatabaseUserInfoProvider>();
            services.AddTransient<MainWindow>();
        }
    }
}

