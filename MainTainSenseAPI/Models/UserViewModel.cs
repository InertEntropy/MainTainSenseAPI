namespace MainTainSenseAPI.Models
{
    public class UserViewModel : BaseViewModels
    {
        public string Id { get; set; } = "";
        public string UserName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string JobTitle { get; set; } = "";  // Assuming you want just the name
        public string Department { get; set; } = "";// Assuming the name here as well 
        public string Email { get; set; } = "";
        public IList<string>? Roles { get; set; }

    }
}
