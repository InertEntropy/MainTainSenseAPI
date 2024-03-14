using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Asset
{
    [Range(1, int.MaxValue, ErrorMessage = "Asset Type ID must be a positive number")]
    public int AssetId { get; set; }

    [Required]
    public int AssetTypeId { get; set; }

    public virtual required AssetType AssetType { get; set; }  

    [StringLength(50)]
    public string? Serialnumber { get; set; }

    public int? AssetLocationId { get; set; }

    public AssetStatus Assetstatus { get; set; }

    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
    public string? AssetDescription { get; set; }

    [Required(ErrorMessage = "Please provide a name for the asset.")]
    [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? AssetName { get; set; }

    public string? InstallDate { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public Location? Location { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = [];
        
}
