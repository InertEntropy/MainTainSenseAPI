using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;

namespace SupervisorsDashboard
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserDal userDal = new UserDal();

            // Attempt Windows authentication first
            if (userDal.IsUserAdmin())
            {
                // Success! 
                MessageBox.Show("Login Successful (Windows Authentication)");
                this.DialogResult = DialogResult.OK; // Indicate success to any parent form 
                this.Close();
                return;
            }

            // Windows Auth failed – provide fallback for default admin
            frmDefaultAdminLogin defaultLogin = new frmDefaultAdminLogin();
            if (defaultLogin.ShowDialog() == DialogResult.OK)
            {
                // Successful default admin login
                MessageBox.Show("Login Successful (Default Admin)");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            // else – default admin login was cancelled or failed; no need to do anything here
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
