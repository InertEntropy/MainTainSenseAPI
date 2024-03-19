using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class ChecklistItem : BaseModel
{
    public int Id { get; set; }

    [Required]
    public int? ChecklistId { get; set; }

    [Required(ErrorMessage = "Please provide a name for the asset.")]
    [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? ChecklistItemsDescription { get; set; } = "  ";

    public YesNo IsCompleted { get; set; } = YesNo.No;

    public int? SortOrder { get; set; }

    public virtual Checklist? Checklist { get; set; }
}
