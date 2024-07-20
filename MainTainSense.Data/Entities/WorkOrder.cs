

namespace MainTainSense.Data
{
    public partial class WorkOrder
    {
        public int Id { get; set; }
        public string WorkOrderDescription { get; set; }
        public string RequestedBy { get; set; }
        public string CreatedDate { get; set; }
        public int AssetId { get; set; }
        public int PriorityId { get; set; }
        public string ScheduledDate { get; set; }
        public string Notes { get; set; }
        public int AssignedTeamId { get; set; }
        public string UserId { get; set; }
        public int StatusId { get; set; }
        public string CompletionDate { get; set; }
        public string DueDate { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}