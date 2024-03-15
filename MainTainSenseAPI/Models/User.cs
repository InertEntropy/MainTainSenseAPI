using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class User
{ 
    public int UserId { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    public string? LastLogin { get; set; }

    [Required]
    public int? DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }

    public virtual ICollection<Message> MessageRecipients { get; set; } = [];

    public virtual ICollection<Message> MessageSenders { get; set; } = [];
}
