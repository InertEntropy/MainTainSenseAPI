using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Controllers;

namespace MainTainSenseAPI.Models
{
    public class CustomRoleStore : BaseController, IRoleStore<ApplicationRole>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly MainTainSenseDataContext _roleStoreContext;
        private readonly IRoleStore<ApplicationRole> _roleStore; 

        public CustomRoleStore(MainTainSenseDataContext context, RoleManager<ApplicationRole> roleManager, Serilog.ILogger logger, IConfiguration configuration)
            : base(context, logger, configuration)

        {
            _roleStoreContext = context;
            _roleManager = roleManager;
            _logger = logger;
            _roleStore = roleStore;
        }
        
        private bool disposed = false; 
        
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here. Example:
                    _context.Dispose();
                }

                // Dispose unmanaged resources here (if any)

                disposed = true; // Mark as disposed
            }
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);

             if (await _roleManager.RoleExistsAsync(role.Name))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role name already exists." });
            }

            // Delegate to RoleManager
            var result = await _roleManager.CreateAsync(role); // Remove cancellationToken
            if (!result.Succeeded)
            {
                var errors = new List<IdentityError>();

                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateName")
                    {
                        errors.Add(new IdentityError { Code = "DUPLICATE_ROLE_NAME", Description = "A role with this name already exists." });
                    }
                    else
                    {
                        // Handle other Identity errors (validation, etc.)
                        errors.Add(new IdentityError { Code = "INVALID_ROLE_DATA", Description = error.Description });
                    }
                }

                return IdentityResult.Failed(errors.ToArray());
            }
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }
            catch (DbUpdateException ex)
            {
                var identityResult = HandleDatabaseException(ex);
                return (IdentityResult)identityResult;
            }
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);

            // Find the role (assuming you want to update based on Id)
            var existingRole = await _context.Roles.FindAsync(role.Id);

            if (existingRole == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            }

            // Update properties
            existingRole.Name = role.Name;
            // ... update other properties as needed ...

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }
            catch (DbUpdateException ex)
            {
                var identityResult = HandleDatabaseException(ex); // Handle database errors, return IdentityResult 
                return (IdentityResult)identityResult;
            }
        }

        public Task<IdentityResult> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);

            if (role.Id == null)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Role ID is not set." }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
        
        public Task<IdentityResult> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);

            if (string.IsNullOrEmpty(role.Name))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Role name is not set." }));
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken) { throw new NotImplementedException(); }

        public Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return _context.Roles
                           .Where(r => r.NormalizedName == normalizedRoleName)
                           .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return _context.Roles.FindAsync(new object[] { roleId }, cancellationToken).AsTask();
        }
        
        public string? GetNormalizedRoleName(ApplicationRole role, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);
            return role.NormalizedName;
        }

        public async Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(roleName);

            role.Name = roleName;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }
            catch (DbUpdateException ex)
            {
                // Consider logging ex for debugging
                return HandleDatabaseException(ex);
            }
        }

        public async Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(normalizedName);

            role.NormalizedName = normalizedName;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }
            catch (DbUpdateException ex)
            {
                // Consider logging ex for debugging
                return HandleDatabaseException(ex);
            }
        }








        Task<string?> IRoleStore<ApplicationRole>.GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        

        Task<string> IRoleStore<ApplicationRole>.GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<string?> IRoleStore<ApplicationRole>.GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IRoleStore<ApplicationRole>.SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IRoleStore<ApplicationRole>.SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
