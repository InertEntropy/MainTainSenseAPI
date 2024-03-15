using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class RolePermission
{ 

    [Required]
    public int? PermissionId { get; set; }

    [Required]
    public int? RoleId { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
