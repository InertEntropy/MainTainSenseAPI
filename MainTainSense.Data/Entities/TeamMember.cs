namespace MainTainSense.Data
{
    public partial class TeamMember
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public int TeamId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

    }
}