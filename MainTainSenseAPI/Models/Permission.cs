using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    [Required]
    public string? PermissionName { get; set; }

    [Required]
    public string? PermissionsDescription { get; set; }
}
