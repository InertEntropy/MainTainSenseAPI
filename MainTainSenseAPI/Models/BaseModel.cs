namespace MainTainSenseAPI.Models
{
    public abstract class BaseModel
    {
        public int IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
