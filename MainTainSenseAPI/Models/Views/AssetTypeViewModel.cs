using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class AssetTypeViewModel : BaseViewModels
    {
        public int Id { get; set; }

        public YesNo IsMachine { get; set; } = YesNo.Yes;

        [Required(ErrorMessage = "Please provide a name for the asset.")]
        [StringLength(20, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? AssetTypeName { get; set; }

        [Required(ErrorMessage = "Please provide a description for the asset type.")]
        [StringLength(50, ErrorMessage = "Asset type description should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? AssetTypeDescription { get; set; }
    }
}
