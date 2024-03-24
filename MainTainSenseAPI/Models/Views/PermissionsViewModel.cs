using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class PermissionsViewModel : BaseViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the permission.")]
        [StringLength(50, ErrorMessage = "Permission Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please provide a description for the permission.")]
        [StringLength(50, ErrorMessage = "Permission description should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string Description { get; set; } = "";
    }
}
