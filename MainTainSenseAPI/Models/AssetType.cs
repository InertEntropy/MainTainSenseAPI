using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class AssetType
{
    public int AssetTypeId { get; set; }

    public YesNo IsMachine { get; set; }

    [Required]
    public string? AssetTypeName { get; set; }

    public string? AssetTypeDescription { get; set; }

    public ActiveStatus Active { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }
}
