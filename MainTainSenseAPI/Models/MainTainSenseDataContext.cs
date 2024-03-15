using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Models;

public partial class MainTainSenseDataContext : DbContext
{
    public MainTainSenseDataContext()
    { 
    }

    public MainTainSenseDataContext(DbContextOptions<MainTainSenseDataContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Accesslevel> Accesslevels { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetType> AssetTypes { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Checklist> Checklists { get; set; }

    public virtual DbSet<ChecklistItem> Checklistitems { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PmChecklist> Pmchecklists { get; set; }

    public virtual DbSet<PmTemplate> Pmtemplates { get; set; }

    public virtual DbSet<PreventiveMaintenance> Preventivemaintenances { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> Rolepermissions { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamMember> Teammembers { get; set; }

    public virtual DbSet<TemplateTask> Templatetasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> Userroles { get; set; }

    public virtual DbSet<WorkOrder> Workorders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=C:\\Sqlite\\MainTainSenseData.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accesslevel>(entity =>
        {
            entity.ToTable("accesslevels");

            entity.Property(e => e.AccessLevelId).HasColumnName("accesslevelid");
            entity.Property(e => e.Accesslevelname)
                .HasColumnType("text(50)")
                .HasColumnName("accesslevelname");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("assets");

            entity.Property(e => e.AssetId).HasColumnName("assetid");
            entity.Property(e => e.AssetDescription).HasColumnName("assetdescription");
            entity.Property(e => e.AssetLocationId).HasColumnName("assetlocationid");
            entity.Property(e => e.AssetName)
                .HasColumnType("text(100)")
                .HasColumnName("assetname");
            entity.Property(e => e.Assetstatus)
                .HasColumnType("text(50)")
                .HasColumnName("assetstatus");
            entity.Property(e => e.AssetTypeId).HasColumnName("assettypeid");
            entity.Property(e => e.InstallDate).HasColumnName("installdate");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.Serialnumber)
                .HasColumnType("text(50)")
                .HasColumnName("serialnumber");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
        });

        modelBuilder.Entity<AssetType>(entity =>
        {
            entity.ToTable("assettypes");

            entity.Property(e => e.AssetTypeId).HasColumnName("assettypeid");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.IsMachine).HasColumnName("ismachine");
            entity.Property(e => e.AssetTypeDescription).HasColumnName("assettypedescription");
            entity.Property(e => e.AssetTypeName)
                .HasColumnType("text(50)")
                .HasColumnName("assettypename");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.ToTable("buildings");

            entity.Property(e => e.BuildingId).HasColumnName("buildingid");
            entity.Property(e => e.BuildingDescription)
                .HasColumnType("text(255)")
                .HasColumnName("buildingdescription");
            entity.Property(e => e.BuildingName)
                .HasColumnType("text(100)")
                .HasColumnName("buildingname");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryid");
            entity.Property(e => e.CategoryDescription).HasColumnName("categorydescription");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
        });

        modelBuilder.Entity<Checklist>(entity =>
        {
            entity.ToTable("checklists");

            entity.Property(e => e.ChecklistId).HasColumnName("checklistid");
            entity.Property(e => e.ChecklistName)
                .HasColumnType("text(50)")
                .HasColumnName("checklistname");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
        });

        modelBuilder.Entity<ChecklistItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.ToTable("checklistitems");

            entity.Property(e => e.ItemId).HasColumnName("itemid");
            entity.Property(e => e.ChecklistId).HasColumnName("checklistid");
            entity.Property(e => e.ChecklistItemsDescription).HasColumnName("checklistitemsdescription");
            entity.Property(e => e.IsCompleted).HasColumnName("iscompleted");
            entity.Property(e => e.SortOrder).HasColumnName("sortorder");

            entity.HasOne(d => d.Checklist).WithMany(p => p.ChecklistItems).HasForeignKey(d => d.ChecklistId);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("departmentid");
            entity.Property(e => e.DepartmentName)
                .HasColumnType("text(50)")
                .HasColumnName("departmentname");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
        });

        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.ToTable("frequency");

            entity.Property(e => e.FrequencyId).HasColumnName("frequencyid");
            entity.Property(e => e.DayofMonth).HasColumnName("dayofmonth");
            entity.Property(e => e.Dayofweek).HasColumnName("dayofweek");
            entity.Property(e => e.FrequencyDescription).HasColumnName("frequencydescription");
            entity.Property(e => e.FrequencyMonth).HasColumnName("frequencymonth");
            entity.Property(e => e.IntervalValue).HasColumnName("intervalvalue");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.TimeUnit).HasColumnName("timeunit");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("locations");

            entity.Property(e => e.LocationId).HasColumnName("locationid");
            entity.Property(e => e.BuildingId).HasColumnName("buildingid");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.LocationDescription).HasColumnName("locationdescription");
            entity.Property(e => e.LocationName)
                .HasColumnType("text(100)")
                .HasColumnName("locationname");
            entity.Property(e => e.LocationPath)
                .HasColumnType("text(255)")
                .HasColumnName("locationpath");
            entity.Property(e => e.ParentLocationId).HasColumnName("parentlocationid");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");

            entity.HasOne(d => d.Building).WithMany(p => p.Locations).HasForeignKey(d => d.BuildingId);

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.ChildLocations).HasForeignKey(d => d.ParentLocationId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessagesId);

            entity.ToTable("messages");

            entity.Property(e => e.MessagesId).HasColumnName("messagesid");
            entity.Property(e => e.CreationTime).HasColumnName("creationtime");
            entity.Property(e => e.IsDeletedForRecipient).HasColumnName("isdeletedforrecipient");
            entity.Property(e => e.IsDeletedForSender).HasColumnName("isdeletedforsender");
            entity.Property(e => e.IsRead).HasColumnName("isread");
            entity.Property(e => e.MessageText).HasColumnName("messagetext");
            entity.Property(e => e.ParenttMessageId).HasColumnName("parenttmessageid");
            entity.Property(e => e.RecipientId).HasColumnName("recipientid");
            entity.Property(e => e.SenderId).HasColumnName("senderid");
            entity.Property(e => e.Subject)
                .HasColumnType("text(50)")
                .HasColumnName("subject");

            entity.HasOne(d => d.ParenttMessage).WithMany(p => p.InverseParenttMessage).HasForeignKey(d => d.ParenttMessageId);

            entity.HasOne(d => d.Recipient).WithMany(p => p.MessageRecipients).HasForeignKey(d => d.RecipientId);

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders).HasForeignKey(d => d.SenderId);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");

            entity.Property(e => e.PermissionId).HasColumnName("permissionid");
            entity.Property(e => e.PermissionName)
                .HasColumnType("text(50)")
                .HasColumnName("permissionname");
            entity.Property(e => e.PermissionsDescription).HasColumnName("permissionsdescription");
        });

        modelBuilder.Entity<PmChecklist>(entity =>
        {
            entity.HasKey(e => e.ChecklistId);

            entity.ToTable("pmchecklists");

            entity.Property(e => e.ChecklistId).HasColumnName("checklistid");
            entity.Property(e => e.PmId).HasColumnName("pmid");

            entity.HasOne(d => d.Pm).WithMany(p => p.PmChecklists).HasForeignKey(d => d.PmId);
        });

        modelBuilder.Entity<PmTemplate>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pmtemplates");

            entity.Property(e => e.AssetTypeId).HasColumnName("assettypeid");
            entity.Property(e => e.IsActive)
                .HasColumnType("boolean")
                .HasColumnName("isactive");
            entity.Property(e => e.PmTemplateDescription).HasColumnName("pmtemplatedescription");
            entity.Property(e => e.TemplateId).HasColumnName("templateid");
            entity.Property(e => e.TemplateName)
                .HasColumnType("text(50)")
                .HasColumnName("templatename");

        });

        modelBuilder.Entity<PreventiveMaintenance>(entity =>
        {
            entity.HasKey(e => e.PmId);

            entity.ToTable("preventivemaintenance");

            entity.Property(e => e.PmId).HasColumnName("pmid");
            entity.Property(e => e.AssetId).HasColumnName("assetid");
            entity.Property(e => e.FrequencyId).HasColumnName("frequencyid");
            entity.Property(e => e.IsActive)
                .HasColumnType("boolean")
                .HasColumnName("isactive");
            entity.Property(e => e.LastCompletedDate).HasColumnName("lastcompleteddate");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.NextDueDate).HasColumnName("nextduedate");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PmDescription).HasColumnName("pmdescription");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.ToTable("priorities");

            entity.Property(e => e.PriorityId).HasColumnName("priorityid");
            entity.Property(e => e.ColorCode)
                .HasColumnType("text(7)")
                .HasColumnName("colorcode");
            entity.Property(e => e.PriorityLevel).HasColumnName("prioritylevel");
            entity.Property(e => e.PriorityName)
                .HasColumnType("text(20)")
                .HasColumnName("priorityname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("roleid");
            entity.Property(e => e.RoleName)
                .HasColumnType("text(50)")
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("rolepermissions");

            entity.Property(e => e.PermissionId).HasColumnName("permissionid");
            entity.Property(e => e.RoleId).HasColumnName("roleid");

            entity.HasOne(d => d.Permission).WithMany().HasForeignKey(d => d.PermissionId);

            entity.HasOne(d => d.Role).WithMany().HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status");

            entity.Property(e => e.StatusId).HasColumnName("statusid");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.StatusDescription).HasColumnName("statusdescription");
            entity.Property(e => e.StatusName)
                .HasColumnType("text(20)")
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("teams");

            entity.Property(e => e.TeamId).HasColumnName("teamid");
            entity.Property(e => e.Department)
                .HasColumnType("text(50)")
                .HasColumnName("department");
            entity.Property(e => e.TeamName)
                .HasColumnType("text(50)")
                .HasColumnName("teamname");
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("teammembers");

            entity.Property(e => e.TeamId).HasColumnName("teamid");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.Team).WithMany().HasForeignKey(d => d.TeamId);

            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TemplateTask>(entity =>
        {
            entity.HasKey(e => e.TemplateId);

            entity.ToTable("templatetasks");

            entity.Property(e => e.TemplateId).HasColumnName("templateid");
            entity.Property(e => e.ChecklistId).HasColumnName("checklistid");
            entity.Property(e => e.FrequencyId).HasColumnName("frequencyid");
            entity.Property(e => e.TemplateTasksDescription).HasColumnName("templatetasksdescription");

            entity.HasOne(d => d.Checklist).WithMany(p => p.TemplateTasks).HasForeignKey(d => d.ChecklistId);

            entity.HasOne(d => d.Frequency).WithMany(p => p.TemplateTasks).HasForeignKey(d => d.FrequencyId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentid");
            entity.Property(e => e.Email)
                .HasColumnType("text(100)")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("text(50)")
                .HasColumnName("firstname");
            entity.Property(e => e.IsActive)
                .HasColumnType("boolean")
                .HasColumnName("isactive");
            entity.Property(e => e.LastLogin).HasColumnName("lastlogin");
            entity.Property(e => e.LastName)
                .HasColumnType("text(50)")
                .HasColumnName("lastname");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
            entity.Property(e => e.UserName)
                .HasColumnType("text(50)")
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("userroles");

            entity.Property(e => e.RoleId).HasColumnName("roleid");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.Role).WithMany().HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.ToTable("workorders");

            entity.Property(e => e.WorkOrderId).HasColumnName("workorderid");
            entity.Property(e => e.AssetId).HasColumnName("assetid");
            entity.Property(e => e.AssignedTeamId).HasColumnName("assignedteamid");
            entity.Property(e => e.CompletionDate).HasColumnName("completiondate");
            entity.Property(e => e.CreatedDate).HasColumnName("createddate");
            entity.Property(e => e.DueDate).HasColumnName("duedate");
            entity.Property(e => e.LastUpdate).HasColumnName("lastupdate");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PriorityId).HasColumnName("priorityid");
            entity.Property(e => e.RequestedBy)
                .HasColumnType("text(50)")
                .HasColumnName("requestedby");
            entity.Property(e => e.ScheduledDate).HasColumnName("scheduleddate");
            entity.Property(e => e.StatusId).HasColumnName("statusid");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text(50)")
                .HasColumnName("updatedby");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.WorkOrderDescription).HasColumnName("workorderdescription");

            entity.HasOne(d => d.Asset).WithMany(p => p.WorkOrders).HasForeignKey(d => d.AssetId);

            entity.HasOne(d => d.Priority).WithMany(p => p.Workorders).HasForeignKey(d => d.PriorityId);

            entity.HasOne(d => d.Status).WithMany(p => p.Workorders).HasForeignKey(d => d.StatusId);
        });
    }
}
