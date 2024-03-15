using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class UserRole
{
    public string? IdentityUserId { get; set; }

    [Required]
    public int? UserId { get; set; }

    public virtual IdentityUser? IdentityUser { get; set; }

    public virtual User? User { get; set; }
}
