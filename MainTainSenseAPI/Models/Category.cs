using MainTainSenseAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Category : IEntityWithId
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    [Required]
    public string? CategoryDescription { get; set; }

    public ActiveStatus IsActive { get; set; }

    public string? UpdatedBy { get; set; }

    public string? LastUpdate { get; set; }
}
