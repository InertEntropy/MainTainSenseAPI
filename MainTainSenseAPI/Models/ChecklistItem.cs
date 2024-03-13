using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class ChecklistItem
{
    public int ItemId { get; set; }

    [Required]
    public int? ChecklistId { get; set; }

    [Required]
    public string? ChecklistItemsDescription { get; set; }

    public int? IsCompleted { get; set; }

    public int? SortOrder { get; set; }

    public virtual Checklist? Checklist { get; set; }
}
