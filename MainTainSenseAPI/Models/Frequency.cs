using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Frequency : BaseModel
{

    public int Id { get; set; }

    [Required]
    public string? FrequencyDescription { get; set; }

    [Required]
    public int? IntervalValue { get; set; }

    [Required]
    public string? TimeUnit { get; set; }

    public int? DayofMonth { get; set; }

    public string? Dayofweek { get; set; }

    public string? FrequencyMonth { get; set; }

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
