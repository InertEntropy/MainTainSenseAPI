using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Checklist : BaseModel
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the checklist .")]
    [StringLength(50, ErrorMessage = "Checklist Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? ChecklistName { get; set; } = "  ";

    public virtual ICollection<ChecklistItem> ChecklistItems { get; set; } = [];

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
