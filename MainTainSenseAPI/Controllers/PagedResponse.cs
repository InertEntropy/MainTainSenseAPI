namespace MainTainSenseAPI.Controllers
{
    public class PagedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T>? Items { get; set; }
        public int? IsActive { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
