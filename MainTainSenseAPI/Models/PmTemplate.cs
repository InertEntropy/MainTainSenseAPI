using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class PmTemplate : IEntityWithId
{
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    [Required]
    public string? TemplateName { get; set; }

    [Required]
    public int? AssetTypeId { get; set; }

    public string? PmTemplateDescription { get; set; }

    public bool? IsActive { get; set; }

    public virtual AssetType? AssetType { get; set; }

}
