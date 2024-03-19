using System.ComponentModel.DataAnnotations;
namespace MainTainSenseAPI.Models
{
    public class Permission : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide a name for the Job Title.")]
        public string Name { get; set; } = ""; // Example: "CanViewOrders", "CanEditProducts"

        public string Description { get; set; } = "";

        public virtual ICollection<AppRole> Roles { get; set; } = new HashSet<AppRole>(); // Notice the Role here
    }
}
