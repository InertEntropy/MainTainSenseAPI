namespace MainTainSenseAPI.Models
{
    public class BaseViewModels
    {
        public int IsActive {  get; set; }  
        public DateTime LastUpdated { get; set; }
        public string? UpdatedBy { get; set; } = null;
    }
}
