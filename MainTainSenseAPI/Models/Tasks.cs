namespace MainTainSenseAPI.Models
{
    public class Tasks
    {
        public int Id { get; set; } //Primary Key
        public int RoleId { get; set; } // Foreign Key
        public ApplicationRole? Role { get; set; }
    }
}
