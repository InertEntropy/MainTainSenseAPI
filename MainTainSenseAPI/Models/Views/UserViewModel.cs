namespace MainTainSenseAPI.Models.Views
{
    public class UserViewModel : BaseViewModels
    {
        public string Id { get; set; } = "";
        public required string? UserName { get; set; } = "";
        public string? FirstName { get; set; } = "";
        public string? LastName { get; set; } = "";
        public string? Email { get; set; } = "";
        public int? JobTitleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RolesId { get; set; }
        public IList<string>? Roles { get; set; }
        public List<SelectListItem>? JobTitles { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        public List<SelectListItem>? AvailableRoles { get; set; }

    }
}
