using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    [Required]
    public string? DepartmentName { get; set; }

    public int? IsActive { get; set; }
}
