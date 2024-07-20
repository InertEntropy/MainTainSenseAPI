

namespace MainTainSense.Data
{
    public partial class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}