using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Status : BaseModel
{
    public int Id { get; set; }

    [Required]
    public string? StatusName { get; set; }

    public string? StatusDescription { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = [];
}
