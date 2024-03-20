using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class AssetViewModels : BaseViewModels
    {
        public int Id { get; set; }

        [Required]
        public int AssetTypeId { get; set; }

        // Option 1: For displaying AssetType Name
        public string? AssetTypeName { get; set; } = "";

        // Option 2: For selecting from a dropdown
        public IEnumerable<SelectListItem>? AvailableAssetTypes { get; set; }

        [StringLength(50)]
        public string? Serialnumber { get; set; } = "";

        public int? AssetLocationId { get; set; }

        // Option 1: For displaying Location Name
        public string? LocationName { get; set; }

        // Option 2: For selecting from a dropdown
        public IEnumerable<SelectListItem>? AvailableLocations { get; set; }

        [Required]
        public int AssetStatusId { get; set; }

        // Option 1: For displaying AssetStatus Name
        public string? AssetStatusName { get; set; } = "";

        // Option 2: For selecting from a dropdown
        public IEnumerable<SelectListItem>? AvailableAssetStatuses { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? AssetDescription { get; set; } = "  ";

        [Required(ErrorMessage = "Please provide a name for the asset.")]
        [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string AssetName { get; set; } = "  ";

        public string? InstallDate { get; set; } = "";
    }
}
