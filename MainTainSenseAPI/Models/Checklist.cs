using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Checklist : BaseModel
{

    public int Id { get; set; }

    [Required]
    public string? ChecklistName { get; set; }

    public virtual ICollection<ChecklistItem> ChecklistItems { get; set; } = [];

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
