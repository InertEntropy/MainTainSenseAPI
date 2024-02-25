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
        public frmManageUsers()
        {
            InitializeComponent();
            
         }   
    }
    private UserDal userNewDal = new UserDal();
    private void frmManageUsers_Load(object sender, EventArgs e)
    {
        LoadUserData();
    }
    private void LoadUserData()
    {
        // Placeholder for now:
        List<User> users = userDal.GetAllUsers();  // You'll need to implement this in UserDAL
        dgvUsers.DataSource = users;
    }

}
