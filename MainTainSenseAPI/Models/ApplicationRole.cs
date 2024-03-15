using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public new int Id { get; set; }
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();

    }
}
