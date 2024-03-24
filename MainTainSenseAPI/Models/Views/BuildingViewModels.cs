using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class BuildingViewModels : BaseViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the asset.")]
        [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "Please provide a name for the asset.")]
        [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string BuildingDescription { get; set; } = "";
    }
}
