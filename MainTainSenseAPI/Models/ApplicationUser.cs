using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; } = "First Name";

        [Required]
        [StringLength(20)]
        public string LastName { get; set; } = "Last Name";

        [Required]
        public int JobTitleId { get; set; }

        public virtual UserJobTitle? JobTitle { get; set; }

        [Required]
        public int DepartmentId { get; set; } = 1;

        public virtual Department? Department { get; set; }

        public int? IsActive { get; set; } = 1;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = "System";

        public ICollection<UserJobTitle>? JobTitles { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<AppRole>? Roles { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<TeamMember>? TeamMembers { get; set; }

    }
}
