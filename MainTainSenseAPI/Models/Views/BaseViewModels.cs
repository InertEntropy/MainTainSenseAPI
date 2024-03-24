namespace MainTainSenseAPI.Models.Views
{
    public class BaseViewModels
    {
        public YesNo IsActive { get; set; } = YesNo.Yes;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; } = "Default";
    }
}
