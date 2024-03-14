using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class PreventiveMaintenance
{
    public int PmId { get; set; }

    [Required]
    public string? PmDescription { get; set; }

    [Required]
    public int? AssetId { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public string? LastCompletedDate { get; set; }

    [Required]
    public string? NextDueDate { get; set; }

    public string? Notes { get; set; }

    [Required]
    public int? FrequencyId { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public virtual ICollection<PmChecklist> PmChecklists { get; set; } = [];
}
