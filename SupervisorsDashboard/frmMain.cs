using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupervisorsDashboard
{
    public partial class frmMain : Form
    {
        private UserDal userDal = new UserDal();
        private User user;
        public bool IsAdmin { get; set; }

        public frmMain()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.PolarisPath))
            {
                lblMainPolarisLocation.Text = Properties.Settings.Default.PolarisPath;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CopyPolarisPath))
            {
                lblMainCopyPolarisLocation.Text = Properties.Settings.Default.CopyPolarisPath;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Get the current Windows username
            string currentUserName = WindowsIdentity.GetCurrent().Name;
            int separatorIndex = currentUserName.IndexOf('\\');
            if (separatorIndex != -1)
            {
                currentUserName = currentUserName.Substring(separatorIndex + 1);
            }

            user = userDal.GetUserByWindowsUsername(currentUserName);
            if (user != null)
            {
                lblWelcome.Text = "Welcome, " + user.UserName; // Use the fetched UserName
            }
            else
            {
                // Handle the case where the user is not found in the database
                lblWelcome.Text = "Welcome! Please contact an administrator."; // A more generic message
            }
            btnSettings.Click += (object? s, EventArgs ea) => { btnSettings_Click(s, ea, user); };

        }

    }

    private void btnSettings_Click(object sender, EventArgs e, User user)
    {
        frmSettings settingsForm = new frmSettings();
        settingsForm.isAdmin = user.IsAdmin;
        settingsForm.Show();
    }
}

