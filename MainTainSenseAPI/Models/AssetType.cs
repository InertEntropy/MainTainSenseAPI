using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class AssetType
{
    public int AssetTypeId { get; set; }

    [Required]
    public string? AssetTypeName { get; set; }

    public string? AssetTypeDescription { get; set; }

    public int? Active { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }
}
