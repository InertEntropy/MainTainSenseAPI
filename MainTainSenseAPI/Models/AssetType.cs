using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class AssetType : BaseModel
{
    public int Id { get; set; }

    public YesNo IsMachine { get; set; } = YesNo.No;

    [Required(ErrorMessage = "Please provide a name for the asset type.")]
    [StringLength(50, ErrorMessage = "Asset type Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? AssetTypeName { get; set; }

    public string? AssetTypeDescription { get; set; }

}
