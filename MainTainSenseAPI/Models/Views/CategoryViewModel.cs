using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class CategoryViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the category.")]
        [StringLength(50, ErrorMessage = "Category Description should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? CategoryDescription { get; set; }
    }
}
