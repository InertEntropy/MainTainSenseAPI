namespace MainTainSense.Data
{
    public partial class TemplateTask
    {
        public int Id { get; set; }
        public string TemplateTasksDescription { get; set; }
        public int FrequencyId { get; set; }
        public int ChecklistId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}