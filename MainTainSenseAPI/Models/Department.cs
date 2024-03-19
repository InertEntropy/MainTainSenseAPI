using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models
{
    public class Department : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the Department.")]
        [StringLength(50, ErrorMessage = "Department Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string Name { get; set; } = "  ";

        public ICollection<ApplicationUser>? Users { get; set; } // Navigation property
    }
}
