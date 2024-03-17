namespace MainTainSenseAPI.Models
{
    public class Permission : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // Example: "CanViewOrders", "CanEditProducts"
        public string Description { get; set; } = "";

        public virtual ICollection<AppRole> Roles { get; set; } = new HashSet<AppRole>(); // Notice the Role here
    }
}
