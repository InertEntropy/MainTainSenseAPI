using System.ComponentModel.DataAnnotations;


namespace MainTainSenseAPI.Models;

public partial class Accesslevel
{ 

    public int AccessLevelId { get; set; }

    [Required]
    public string? Accesslevelname { get; set; }
}
