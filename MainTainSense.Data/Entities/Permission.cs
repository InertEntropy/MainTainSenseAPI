 
namespace MainTainSense.Data
{
    public class Permission 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
