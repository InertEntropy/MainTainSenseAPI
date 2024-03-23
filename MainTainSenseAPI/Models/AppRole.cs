using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class AppRole : IdentityRole
    {
        public int? IsActive { get; set; } = 1;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = "System";
        public virtual ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>(); // Initialize to avoid null issues
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
