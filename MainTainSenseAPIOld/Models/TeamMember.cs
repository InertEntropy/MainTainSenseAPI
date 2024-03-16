namespace MainTainSenseAPI.Models;

public partial class TeamMember
{
    public int? TeamId { get; set; }

    public int? UserId { get; set; }

    public virtual Team? Team { get; set; }

    public virtual User? User { get; set; }
}
