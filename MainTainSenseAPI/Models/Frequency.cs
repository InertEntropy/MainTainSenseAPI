using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Frequency : BaseModel
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the asset.")]
    [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? FrequencyDescription { get; set; }

    [Required]
    public int? IntervalValue { get; set; } = default;

    [Required]
    public string? TimeUnit { get; set; }

    public int? DayofMonth { get; set; }

    public string? Dayofweek { get; set; }

    public string? FrequencyMonth { get; set; }

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
