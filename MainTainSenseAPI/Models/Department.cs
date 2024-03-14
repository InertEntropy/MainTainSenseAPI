using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Department : IEntityWithId
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    [Required]
    public string? DepartmentName { get; set; }

    public int? IsActive { get; set; }
}
