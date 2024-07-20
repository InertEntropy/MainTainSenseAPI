using System.Data.Entity;

namespace MainTainSense.Data
{
    public class MainTainSenseDbContext : DbContext
    {
        public DbSet<Assets> Assets { get; set; }
        public DbSet<AssetTypes> AssetTypes { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Frequency> Frequency { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PmChecklist> PmChecklists { get; set; }
        public DbSet<PmTemplate> PmTemplates { get; set; }
        public DbSet<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> AssetStatus { get; set; }
        public DbSet<TaskName> TaskNames { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TemplateTask> TemplateTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserJobTitles> UserJobTitles { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
    }
}
