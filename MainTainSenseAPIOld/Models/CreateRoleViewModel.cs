using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string RoleName { get; set; }

        [RegularExpression("^[a-zA-Z0-9_-]*$")] // Only letters, numbers, '_', and '-' allowed
        public string Description { get; set; }
    }
}
