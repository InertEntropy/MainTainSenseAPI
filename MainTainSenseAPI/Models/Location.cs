using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Location : BaseModel
{
    public int Id { get; set; }

    [Required]
    public string? LocationName { get; set; }

    [Required]
    public string? LocationDescription { get; set; }

    public int? BuildingId { get; set; }

    public string? LocationPath { get; set; }

    public int? ParentLocationId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Location? ParentLocation { get; set; }

    public virtual ICollection<Location> ChildLocations { get; set; } = [];  // Use a more descriptive name
}