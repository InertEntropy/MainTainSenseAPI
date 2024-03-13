using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Location
{
    public int LocationId { get; set; }

    [Required]
    public string? LocationName { get; set; }

    [Required]
    public string? LocationDescription { get; set; }

    public int? BuildingId { get; set; }

    public double? IsActive { get; set; }

    public string? LocationPath { get; set; }

    public int? ParentLocationId { get; set; }

    public int? IsMachine { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public virtual Building? Building { get; set; }

    public virtual ICollection<Location> InverseParentlocation { get; set; } = new List<Location>();

    public virtual Location? ParentLocation { get; set; }
}
