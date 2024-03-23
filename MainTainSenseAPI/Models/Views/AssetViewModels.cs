using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class AssetViewModels : BaseViewModels
    {
        public int Id { get; set; }

        [Required]
        public int AssetTypeId { get; set; }

        // Dropdown Data 
        public IEnumerable<SelectListItem>? AvailableAssetTypes { get; set; }

        [StringLength(50)]
        public string? Serialnumber { get; set; } = "";

        public int? AssetLocationId { get; set; }
        public IEnumerable<SelectListItem>? AvailableLocations { get; set; }

        [Required]
        public int AssetStatusId { get; set; }
        public IEnumerable<SelectListItem>? AvailableAssetStatuses { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? AssetDescription { get; set; } = "  ";

        [Required(ErrorMessage = "Please provide a name for the asset.")]
        [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? AssetName { get; set; }

        public string? InstallDate { get; set; }
    }
}
