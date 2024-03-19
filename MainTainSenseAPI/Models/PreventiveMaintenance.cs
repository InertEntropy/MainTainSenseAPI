using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class PreventiveMaintenance : BaseModel
{

    public int Id { get; set; }

    [Required]
    public string? PmDescription { get; set; }

    [Required]
    public int? AssetId { get; set; }

    [Required]
    public string? LastCompletedDate { get; set; }

    [Required]
    public string? NextDueDate { get; set; }

    public string? Notes { get; set; }

    [Required]
    public int? FrequencyId { get; set; }

    public virtual ICollection<PmChecklist> PmChecklists { get; set; } = [];
}
