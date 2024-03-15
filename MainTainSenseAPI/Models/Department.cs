using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Department 
{ 

    public int Id { get; set; }

    [Required]
    public string? DepartmentName { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
