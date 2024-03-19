using System.ComponentModel.DataAnnotations;
namespace MainTainSenseAPI.Models
{
    public class UserJobTitle : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the Job Title.")]
        public string Name { get; set; } = "";
        public ICollection<ApplicationUser>? Users { get; set; } // Navigation property
    }
}
