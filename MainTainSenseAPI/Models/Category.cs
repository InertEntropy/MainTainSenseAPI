
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Category : BaseModel
{
    public int Id { get; set; }

    [Required]
    public string? CategoryDescription { get; set; }
}
