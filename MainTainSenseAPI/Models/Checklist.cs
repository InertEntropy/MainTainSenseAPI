using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Checklist : IEntityWithId
{
    public int Id { get; set; }

    public int ChecklistId { get; set; }

    [Required]
    public string? ChecklistName { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<ChecklistItem> ChecklistItems { get; set; } = [];

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
