namespace MainTainSenseAPI.Models.Views;

public partial class TeamMember : BaseViewModels
{
    
    public int? Id { get; set; }

    public string? UserId { get; set; }

    public virtual Team? Team { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
