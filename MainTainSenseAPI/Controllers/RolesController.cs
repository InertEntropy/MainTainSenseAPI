using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    public class RolesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(ILogger<RolesController> logger, IConfiguration configuration, ApplicationDbContext context, RoleManager<AppRole> roleManager, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _roleManager = roleManager;
        }
        [HttpGet("api/roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                                      .Select(r => new
                                      {
                                          r.Id,
                                          r.Name,
                                      })
                                      .ToListAsync();

            return Ok(roles);
        }
        [HttpPost("api/roles")]
        public async Task<IActionResult> CreateRole([FromBody] RolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Leverages your BaseController error handling
            }

            var role = new AppRole
            {
                Name = model.Name,
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                logger.LogInformation("Role created: {roleName}", role.Name); // Log
                return Ok();
            }
            else
            {
                return HandleIdentityError(result);
            }
        }

        [HttpGet("api/roles/{roleId}/permissions")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            
            var permissions = await _context.Permissions
                                        .Where(p => role.Permissions.Contains(p))
                                        .ToListAsync();

            return Ok(permissions);
        }

        [HttpPost("api/roles/{roleId}/permissions")]
        public async Task<IActionResult> AssignPermissionToRole(string roleId, [FromBody] int permissionId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            if (role.Permissions.Contains(permission))
            {
                return BadRequest("Role already has this permission");
            }

            role.Permissions.Add(permission);
            await _context.SaveChangesAsync(); // Or update via roleManager if available

            logger.LogInformation("Permission {permissionId} assigned to role {roleId}", permissionId, roleId);

            return Ok();
        }

        [HttpDelete("api/roles/{roleId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, int permissionId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found");
            }
            if (_context == null)
            {
                // Handle the case where _context is null 
                return StatusCode(500, "Database context error - contact administrator");
            }
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            if (!role.Permissions.Contains(permission))
            {
                return BadRequest("Role doesn't have this permission");
            }

            role.Permissions.Remove(permission);
            await _context.SaveChangesAsync(); // Assuming sufficient permissions to update

            logger.LogInformation("Permission {permissionId} removed from role {roleId}", permissionId, roleId);

            return Ok();
        }
    }
}

