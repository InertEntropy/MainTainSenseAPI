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
        public DbSet<JobTitle> JobTitles { get; set; }

        //public DbSet<ApplicationUser> Users { get; set; }
        // ... other DbSets ...

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
        }
    }
}
