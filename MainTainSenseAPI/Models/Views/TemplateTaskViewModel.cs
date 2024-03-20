using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class TemplateTask : BaseViewModels
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a description.")]
    [StringLength(50, ErrorMessage = "Description should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? TemplateTasksDescription { get; set; }

    public int? FrequencyId { get; set; }

    public int? ChecklistId { get; set; }

    public virtual Checklist? Checklist { get; set; }

    public virtual Frequency? Frequency { get; set; }
}
