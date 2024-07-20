namespace MainTainSense.Data
{
    public class Assets
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetDescription { get; set; }
        public int LocationId { get; set; }
        public int StatusId { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string InstallDate { get; set; }
        public int VisibleProduction {  get; set; }
        public bool IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public AssetTypes AssetType { get; set; }
        public Location Location { get; set; }
        public Status Status { get; set; }
    }
}
