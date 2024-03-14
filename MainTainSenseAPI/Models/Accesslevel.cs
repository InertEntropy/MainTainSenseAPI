using System.ComponentModel.DataAnnotations;
using MainTainSenseAPI.Contracts;

namespace MainTainSenseAPI.Models;

public partial class Accesslevel : IEntityWithId
{
    public int Id { get; set; }

    public int AccessLevelId { get; set; }

    [Required]
    public string? Accesslevelname { get; set; }
}
