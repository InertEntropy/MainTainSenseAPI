
namespace MainTainSense.Data
{
    public partial class Building
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDescription { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}