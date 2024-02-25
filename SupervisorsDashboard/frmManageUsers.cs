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
    public partial class frmManageUsers : Form
    {
        // Form-level variables
        private UserDal userDal = new UserDal();

        // Constructor
        public frmManageUsers()
        {
            InitializeComponent();
            dgvUsers.DataSource = userDal.GetAllUsers();
        }

        // Event handlers
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        // Other event handlers (e.g., button clicks)

        // Helper functions
        private void LoadUserData()
        {
            List<User> users = userDal.GetAllUsers();
            dgvUsers.DataSource = users;
        }
    }
}
