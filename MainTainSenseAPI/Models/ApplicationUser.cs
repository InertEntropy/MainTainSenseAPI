using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int JobTitleId { get; set; }
        public virtual UserJobTitle? JobTitle { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public int? IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;

        public ICollection<UserJobTitle>? JobTitles { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<AppRole>? Roles { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<TeamMember>? TeamMembers { get; set; }
    }
}
