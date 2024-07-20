namespace MainTainSense.Data
{
    public partial class Priority
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public int PriorityLevel { get; set; }
        public string ColorCode { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}