

namespace MainTainSense.Data
{
    public partial class PmChecklist
    {
        public int Id { get; set; }
        public int? PmId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}