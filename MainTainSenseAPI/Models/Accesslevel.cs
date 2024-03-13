using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Accesslevel
{
    public int Accesslevelid { get; set; }

    [Required]
    public string? Accesslevelname { get; set; }
}
