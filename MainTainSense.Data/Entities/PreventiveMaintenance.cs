namespace MainTainSense.Data
{
    public partial class PreventiveMaintenance
    {
        public int Id { get; set; }
        public string PmDescription { get; set; }
        public int AssetId { get; set; }
        public string LastCompletedDate { get; set; }
        public string NextDueDate { get; set; }
        public string Notes { get; set; }
        public int FrequencyId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}