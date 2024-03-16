using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Team
{ 
    public int Id { get; set; }

    [Required]
    public string? TeamName { get; set; }

    public string? Department { get; set; }
}
