using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{

    public class PermissionsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsController(ILogger<PermissionsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedPermissions(int pageNumber = 1, int pageSize = 10,
                int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Permission> query = _context.Permissions;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                query = query.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            var locations = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return Ok(locations);
        }

        // Get Permissions by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var location = await _context.Permissions.FindAsync(id);

            if (location == null)
            {
                return NotFound("Permissions not found");
            }

            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermissions([FromBody] PermissionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userpermission = new Permission
            {
                Name = model.Name,
                Description = model.Description,

                IsActive = model.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Permissions.Add(userpermission);
                await _context.SaveChangesAsync();

                logger.LogInformation("Permissions created with ID: {id}", userpermission.Id);
                return CreatedAtAction(nameof(GetPermissions), new { id = userpermission.Id }, userpermission);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating permission.");
                return StatusCode(500, "Error saving permission to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(userpermission.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            logger.LogError("Create Permissions: unhandled error occured");
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermissions(int id, [FromBody] PermissionsViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound("Permissions not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.Permissions.FirstOrDefaultAsync(a => a.Id == id);

            if (entity == null)
            {
                return NotFound("Permissions not found");
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.LastUpdated = DateTime.UtcNow;
            entity.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Permissions Updated: {id}", entity.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating permission.");
                return StatusCode(500, "Error saving updated permission to database.");
            }

            return NoContent();
        }
    }
}
