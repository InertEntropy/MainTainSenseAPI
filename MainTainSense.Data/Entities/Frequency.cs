namespace MainTainSense.Data
{
    public partial class Frequency
    {
        public int Id { get; set; }
        public string FrequencyDescription { get; set; }
        public int IntervalValue { get; set; } = default;
        public string TimeUnit { get; set; }
        public int DayofMonth { get; set; }
        public string Dayofweek { get; set; }
        public string FrequencyMonth { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}