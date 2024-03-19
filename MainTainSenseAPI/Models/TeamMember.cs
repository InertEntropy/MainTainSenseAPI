namespace MainTainSenseAPI.Models;

public partial class TeamMember : BaseModel
{
    
    public int? Id { get; set; }

    public string? UserId { get; set; }

    public virtual Team? Team { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
