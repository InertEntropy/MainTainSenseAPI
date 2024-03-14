using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Frequency : IEntityWithId
{
    public int Id { get; set; }

    public int FrequencyId { get; set; }

    [Required]
    public string? FrequencyDescription { get; set; }

    [Required]
    public int? IntervalValue { get; set; }

    [Required]
    public string? TimeUnit { get; set; }

    public int? IsActive { get; set; }

    public int? DayofMonth { get; set; }

    public string? Dayofweek { get; set; }

    public string? FrequencyMonth { get; set; }

    public virtual ICollection<TemplateTask> TemplateTasks { get; set; } = [];
}
