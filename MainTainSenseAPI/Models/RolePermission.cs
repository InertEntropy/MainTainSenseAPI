using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class RolePermission : IEntityWithId
{
    public int Id { get; set; }

    [Required]
    public int? PermissionId { get; set; }

    [Required]
    public int? RoleId { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
