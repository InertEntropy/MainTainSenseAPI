namespace MainTainSenseAPI.Models.Views
{
    public class BaseViewModels
    {
        public int IsActive { get; set; } = 1;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; } = "Default";
    }
}
