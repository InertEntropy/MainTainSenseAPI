using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Team : BaseModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the Team.")]
    [StringLength(50, ErrorMessage = "Team Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? TeamName { get; set; }

    public string? Department { get; set; }
}
