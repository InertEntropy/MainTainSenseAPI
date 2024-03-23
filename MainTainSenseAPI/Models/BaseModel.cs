namespace MainTainSenseAPI.Models
{
    public abstract class BaseModel
    {
        public YesNo IsActive { get; set; } = YesNo.Yes;
        public DateTime? LastUpdated { get; set; } 
        public string? UpdatedBy { get; set; } = null;
    }
}
