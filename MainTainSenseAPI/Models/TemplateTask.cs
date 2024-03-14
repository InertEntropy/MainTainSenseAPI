using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class TemplateTask : IEntityWithId
{
    public int Id { get; set; }

    public int TemplateId { get; set; }

    [Required]
    public string? TemplateTasksDescription { get; set; }

    public int? FrequencyId { get; set; }

    public int? ChecklistId { get; set; }

    public virtual Checklist? Checklist { get; set; }

    public virtual Frequency? Frequency { get; set; }
}
