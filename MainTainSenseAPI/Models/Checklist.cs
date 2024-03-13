using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Checklist
{
    public int ChecklistId { get; set; }

    [Required]
    public string? ChecklistName { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = new List<TemplateTask>();
}
