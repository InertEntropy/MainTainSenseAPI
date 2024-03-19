using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class PmChecklist : BaseModel
{

    public int Id { get; set; }
    [Required]
    public int? PmId { get; set; }

    public virtual PreventiveMaintenance? Pm { get; set; }
}
