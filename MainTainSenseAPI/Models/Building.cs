using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Building : BaseModel
{ 
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the building.")]
    [StringLength(50, ErrorMessage = "Building name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? BuildingName { get; set; } = "  ";

    public string BuildingDescription { get; set; } = "";

    public virtual ICollection<Location> Locations { get; set; } = [];
}