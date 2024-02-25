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
    public partial class frmSettings : Form
    {
        public frmSettings(string currentPolarisPath, string currentCopyPolarisPath)
        {
            InitializeComponent();
            txtPolarisPath.Text = currentPolarisPath;
            txtPolarisCopyPath.Text = currentCopyPolarisPath;

        }

        private void btnBrowsePolaris_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Access Database Files (*.mdb)|*.mdb"; // Filter for Access files
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPolarisPath.Text = fileDialog.FileName;
            }
        }

        private void btnBrowsePolarisCopy_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Access Database Files (*.mdb)|*.mdb"; // Filter for Access files
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPolarisCopyPath.Text = fileDialog.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        //    if (IsUserAdmin()) // Implement the IsUserAdmin function 
        //   {
        //        Properties.Settings.Default.PolarisPath = txtPolarisPath.Text;
        //        Properties.Settings.Default.CopyPolarisPath = txtPolarisCopyPath.Text;
        //        Properties.Settings.Default.Save();
        //        MessageBox.Show("Settings saved!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("You do not have permissions to modify these settings.");
        //    }
        }
        public string PolarisPath
        {
            get { return txtPolarisPath.Text; }
        }
        public string CopyPolarisPath
        {
            get { return txtPolarisCopyPath.Text; }
        }
    }
}
