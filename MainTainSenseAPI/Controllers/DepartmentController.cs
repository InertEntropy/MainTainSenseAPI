using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentsController(ILogger<DepartmentsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedDepartments(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Department> departmentQuery = _context.Departments;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                departmentQuery = departmentQuery.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No)); // **Removed assignment** 
            }

            int totalCount = await _context.Departments.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var departments = await departmentQuery
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<Department> // Assuming you have a 'Department' entity
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Department>)departments
            };

            return Ok(response);
        }


        // Get Department by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Department not found");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department
            {
                Name = model.Name,
                IsActive = YesNo.Yes,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                logger.LogInformation("Department created with ID: {Name}", department.Id);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating department.");
                return StatusCode(500, "Error saving department to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(department.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound("Department not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var departments = await _context.Departments.FirstOrDefaultAsync(a => a.Id == id);

            if (departments == null)
            {
                return NotFound("Department not found");
            }

            departments.Name = model.Name;
            departments.IsActive = model.IsActive;
            departments.LastUpdated = DateTime.UtcNow;
            departments.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Department Updated: {DepartmentName}", departments.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating department.");
                return StatusCode(500, "Error saving updated department to database.");
            }

            return NoContent();
        }
    }
}


