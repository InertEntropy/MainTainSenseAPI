using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class ChecklistViewModel : BaseViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the checklist.")]
        [StringLength(50, ErrorMessage = "Checklist Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? ChecklistName { get; set; }
    }
}
