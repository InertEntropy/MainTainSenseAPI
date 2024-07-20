namespace MainTainSense.Data
{
    public partial class ChecklistItem
    {
        public int Id { get; set; }
        public int ChecklistId { get; set; }
        public string ChecklistItemsDescription { get; set; }
        public int IsCompleted { get; set; }
        public int SortOrder { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}