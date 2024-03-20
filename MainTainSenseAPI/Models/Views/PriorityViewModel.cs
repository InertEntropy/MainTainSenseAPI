using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class Priority : BaseViewModels
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the priority.")]
    [StringLength(50, ErrorMessage = "Priority Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? PriorityName { get; set; }

    [Required]
    public int? PriorityLevel { get; set; }

    public string? ColorCode { get; set; }
}
