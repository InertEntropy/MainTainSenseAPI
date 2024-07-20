namespace MainTainSense.Data
{
    public partial class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public int BuildingId { get; set; }
        public string LocationPath { get; set; }
        public int ParentLocationId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}