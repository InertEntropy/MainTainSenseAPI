using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Role : IEntityWithId
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    [Required]
    public string? RoleName { get; set; }
}
