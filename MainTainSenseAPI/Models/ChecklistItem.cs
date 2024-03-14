using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class ChecklistItem : IEntityWithId
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    [Required]
    public int? ChecklistId { get; set; }

    [Required]
    public string? ChecklistItemsDescription { get; set; }

    public int? IsCompleted { get; set; }

    public int? SortOrder { get; set; }

    public virtual Checklist? Checklist { get; set; }
}
