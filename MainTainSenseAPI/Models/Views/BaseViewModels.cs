namespace MainTainSenseAPI.Models.Views
{
    public class BaseViewModels
    {
        public int IsActive { get; set; } = 0;
        public DateTime LastUpdated { get; set; }
        public string? UpdatedBy { get; set; } = null;
    }
}
