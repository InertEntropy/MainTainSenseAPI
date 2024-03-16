using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Building
{
    public int Id { get; set; }
    
    [Required]
    public string? BuildingName { get; set; }

    public string? BuildingDescription { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public ActiveStatus IsActive { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = [];
}
