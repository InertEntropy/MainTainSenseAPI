using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class ChecklistItemViewModel : BaseViewModels
    {
        public int Id { get; set; }

        [Required]
        public int? ChecklistId { get; set; }

        [Required(ErrorMessage = "Please provide a name for the checklist item.")]
        [StringLength(50, ErrorMessage = "Checklist item name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? ChecklistItemsDescription { get; set; }

        public YesNo IsCompleted { get; set; } = YesNo.No;

        public int? SortOrder { get; set; }
    }
}
