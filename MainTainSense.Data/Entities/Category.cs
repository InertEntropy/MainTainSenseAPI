

namespace MainTainSense.Data
{
    public partial class Category
    {
        public int Id { get; set; }
        public string CategoryDescription { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}