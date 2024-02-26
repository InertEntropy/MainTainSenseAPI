using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SupervisorsDashboard.UserDal;

namespace SupervisorsDashboard
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsEnabled { get; set; }
        public UserRole Role { get; set; }
        
    }
}
