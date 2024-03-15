using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? LastLogin { get; set; }
        public int DepartmentId { get; set; }
        public int IsActive { get; set; }
        public string? Email { get; set; }
        public string? UpdatedBy { get; set; }
        public string? LastUpdate { get; set; }
    }
}
