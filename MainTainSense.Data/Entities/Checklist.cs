namespace MainTainSense.Data
{
    public partial class Checklist
    {
        public int Id { get; set; }
        public string ChecklistName { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}