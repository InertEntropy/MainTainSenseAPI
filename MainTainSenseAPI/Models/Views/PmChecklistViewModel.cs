using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class PmChecklist : BaseViewModels
{

    public int Id { get; set; }
    [Required]
    public int? PmId { get; set; }

    public virtual PreventiveMaintenance? Pm { get; set; }
}
