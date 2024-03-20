using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class PreventiveMaintenance : BaseViewModels
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the PM.")]
    [StringLength(50, ErrorMessage = "Asset Description should be between 2 and 50 characters long.", MinimumLength = 2)]
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

}
