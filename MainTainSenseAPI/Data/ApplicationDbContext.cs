using MainTainSenseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MainTainSenseAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    { 
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserJobTitle> UserJobTitles { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PmChecklist> PmChecklists { get; set; }
        public DbSet<PmTemplate> PmTemplates { get; set; }
        public DbSet<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TemplateTask> TemplateTasks { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure the base logic runs as well

            builder.Entity<AppRole>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany().HasForeignKey("RoleId").IsRequired(),
                    p => p.HasOne<AppRole>().WithMany().HasForeignKey("PermissionId"),
                    je =>
                    {
                        je.HasKey("RoleId", "PermissionId");
                    });

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.JobTitle)
                .WithMany(jt => jt.Users)
                .HasForeignKey(u => u.JobTitleId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId);

            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.Messages)  
                .HasForeignKey(m => m.RecipientId);

            builder.Entity<Message>()
                .Ignore(m => m.Sender);

            builder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u =>u.TeamMembers) // Adjust based on the relationship type
                .HasForeignKey(tm => tm.UserId);

        }
        
    }
}
