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
    public partial class frmDefaultAdminLogin : Form
    {
        public frmDefaultAdminLogin()
        {
            InitializeComponent();
        }

        private void btnOKDefaultAdmin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            UserDal userDal = new UserDal();
            if (userDal.AuthenticateUser(username, password))
            {
                // Authentication successful
                this.DialogResult = DialogResult.OK; // Signal success to the main login form 
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
                // Consider clearing the password field
                txtPassword.Clear();
            }
        }

        private void btnCancelDefaultAdmin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmDefaultAdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
