using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class UserJobTitleViewModel : BaseModel // Inherit from your BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the job title.")]
        [StringLength(50, ErrorMessage = "Job title Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? Name { get; set; }

        public ICollection<ApplicationUser>? Users { get; set; } 
    }
}
