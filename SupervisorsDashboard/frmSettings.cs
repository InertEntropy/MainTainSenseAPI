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
        public bool isAdmin { get; set; }


        public frmSettings()
        {
            InitializeComponent();

        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            pnlAdminSettings.Visible = isAdmin;
            btnBrowsePolaris.Enabled = isAdmin;
            btnBrowsePolarisCopy.Enabled = isAdmin;

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
