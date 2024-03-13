using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Role
{
    public int RoleId { get; set; }

    [Required]
    public string? RoleName { get; set; }
}
