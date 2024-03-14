using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Status
{
    public int StatusId { get; set; }

    [Required]
    public string? StatusName { get; set; }

    public string? StatusDescription { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<WorkOrder> Workorders { get; set; } = [];
}
