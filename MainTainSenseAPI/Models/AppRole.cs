using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class AppRole : IdentityRole
    {
        public int IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public virtual ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>(); // Initialize to avoid null issues
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
