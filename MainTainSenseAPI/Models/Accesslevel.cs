using System.ComponentModel.DataAnnotations;


namespace MainTainSenseAPI.Models;

public partial class Accesslevel
{ 

    public int Id { get; set; }

    [Required]
    public string? Accesslevelname { get; set; }
}
