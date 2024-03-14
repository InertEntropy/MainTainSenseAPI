using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Building
{
    public int BuildingId { get; set; }
    
    [Required]
    public string? BuildingName { get; set; }

    public string? BuildingDescription { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = [];
}
