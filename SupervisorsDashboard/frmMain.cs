using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupervisorsDashboard
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            //UserDal userDal = new UserDal();
            //userDal.CreateDefaultAdmin();
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

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings settingsForm = new frmSettings(Properties.Settings.Default.PolarisPath, Properties.Settings.Default.PolarisPath);
            settingsForm.ShowDialog();
            // Load settings if they exist
            if (!string.IsNullOrEmpty(settingsForm.PolarisPath))
            {
                lblMainPolarisLocation.Text = settingsForm.PolarisPath;
            }
            if (!string.IsNullOrEmpty(settingsForm.CopyPolarisPath))
            {
                lblMainCopyPolarisLocation.Text = Properties.Settings.Default.PolarisPath;
            }
        }
    }
}
