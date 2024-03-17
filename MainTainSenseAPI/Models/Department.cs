namespace MainTainSenseAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<ApplicationUser>? Users { get; set; } // Navigation property
    }
}
