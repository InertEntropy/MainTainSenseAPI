using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Location : BaseModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the location.")]
    [StringLength(50, ErrorMessage = "Location Name should be between 2 and 50 characters long.", MinimumLength = 2)]
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