
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Category : BaseModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the category.")]
    [StringLength(50, ErrorMessage = "Category name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? CategoryDescription { get; set; }
}
