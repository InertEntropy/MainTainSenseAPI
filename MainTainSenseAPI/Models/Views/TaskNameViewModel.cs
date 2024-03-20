namespace MainTainSenseAPI.Models.Views
{
    public class TaskName : BaseViewModels
    {
        public int Id { get; set; } //Primary Key
        public int RoleId { get; set; } // Foreign Key
        public AppRole? Role { get; set; }
    }
}