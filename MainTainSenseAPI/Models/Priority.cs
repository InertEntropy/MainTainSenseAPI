﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Priority
{
    public int PriorityId { get; set; }

    [Required]
    public string? PriorityName { get; set; }

    [Required]
    public int? PriorityLevel { get; set; }

    public string? ColorCode { get; set; }

    public virtual ICollection<WorkOrder> Workorders { get; set; } = [];
}
