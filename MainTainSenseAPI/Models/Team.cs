using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Team : IEntityWithId
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    [Required]
    public string? TeamName { get; set; }

    public string? Department { get; set; }
}
