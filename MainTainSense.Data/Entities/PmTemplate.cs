namespace MainTainSense.Data
{
    public partial class PmTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public int AssetTypeId { get; set; }
        public string PmTemplateDescription { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}