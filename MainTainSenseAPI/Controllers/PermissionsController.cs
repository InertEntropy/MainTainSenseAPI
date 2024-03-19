using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MainTainSenseAPI.Controllers
{

    public class PermissionsController : BaseController
    {
        private readonly ApplicationDbContext _context; // Or your relevant data access mechanism

        public PermissionsController(ILogger<PermissionsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // 1. Get all permissions
        [HttpGet("api/permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _context.Permissions
                                        .Select(p => new { p.Id, p.Name, p.Description })
                                        .ToListAsync();
            return Ok(permissions);
        }

        // 2. Create a new permission
        [HttpPost("api/permissions")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permission = new Permission
            {
                Name = model.Name,
                Description = model.Description
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            logger.LogInformation ("Permission created: {Name}", permission.Name); // Log
            return CreatedAtAction(nameof(GetPermissions), new { id = permission.Id }, permission);
        }
        [HttpPut("api/permissions/{permissionId}")]
        public async Task<IActionResult> UpdatePermission(int permissionId, [FromBody] PermissionsViewModel model)
        {
            // 1. Find the existing permission
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null)
            {
                return NotFound("Permission not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 3. Update properties
            permission.Name = model.Name;
            permission.Description = model.Description;
            logger.LogInformation("Permission Updated: {Name}", permission.Name);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(permission.Id); // Delegate to BaseController 
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);  // Pass the exception 
            }

            // 5. Success Response (unchanged)
            return NoContent();
        }
    }
}
