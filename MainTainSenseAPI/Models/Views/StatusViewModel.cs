using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class Status : BaseViewModels
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide a name for the Status.")]
    [StringLength(50, ErrorMessage = "Status Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? StatusName { get; set; }

    public string? StatusDescription { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = [];
}
