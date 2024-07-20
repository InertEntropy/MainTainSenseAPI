namespace MainTainSense.Data
{
    public partial class Status
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}