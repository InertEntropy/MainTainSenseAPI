using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class AssetType : BaseModel
{
    public int Id { get; set; }

    public YesNo IsMachine { get; set; }

    [Required]
    public string? AssetTypeName { get; set; }

    public string? AssetTypeDescription { get; set; }

}
