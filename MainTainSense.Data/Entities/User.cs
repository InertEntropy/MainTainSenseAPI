namespace MainTainSense.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int JobTitleId { get; set; }
        public int DepartmentId { get; set; }
        public int IsActive { get; set; }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }
        public UserJobTitles JobTitle { get; set; }
        public Departments Department { get; set; }
    }
}
