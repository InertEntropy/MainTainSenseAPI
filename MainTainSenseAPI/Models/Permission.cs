using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Permission : IEntityWithId
{
    public int Id { get; set; }

    public int PermissionId { get; set; }

    [Required]
    public string? PermissionName { get; set; }

    [Required]
    public string? PermissionsDescription { get; set; }
}
