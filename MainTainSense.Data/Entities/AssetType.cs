
namespace MainTainSense.Data
{
    public class AssetTypes
    {
        public int Id { get; set; }
        public int IsMachine { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetTypeDescription { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}