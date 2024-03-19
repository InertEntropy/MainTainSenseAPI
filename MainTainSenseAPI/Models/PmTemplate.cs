
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class PmTemplate : BaseModel
{

    public int? Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the template.")]
    [StringLength(50, ErrorMessage = "Template Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? TemplateName { get; set; }

    [Required]
    public int? AssetTypeId { get; set; }

    public string? PmTemplateDescription { get; set; }

    public virtual AssetType? AssetType { get; set; }

}