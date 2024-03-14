using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization; // Add if using Authorize

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly MainTainSenseDataContext _context;

        public CategoryController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(ActiveStatus? isActive) // Optional 'isActive' parameter
        {
            var query = _context.Categories.AsQueryable(); // Start with a queryable object

            if (isActive.HasValue) // If the 'isActive' parameter is provided
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id, ActiveStatus? isActive) // Optional 'isActive' parameter
        {
            var query = _context.Categories.AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive); // Direct comparison with integers
            }

            var category = await query.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) { return NotFound(); }
            return category;
        }

        [Authorize] // Assuming you want creation to be authorized
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "One or more validation errors occurred.",
                    Detail = "Please check the following fields:",
                };

                problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();

                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        // Check if errorsDict is null before using it
                        if (problemDetails.Extensions["errors"] is not Dictionary<string, string[]> errorsDict)
                        {
                            errorsDict = [];
                            problemDetails.Extensions["errors"] = errorsDict;
                        }

                        errorsDict.Add(entry.Key, entry.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    }
                }
                return BadRequest(problemDetails); // Returns custom error response
            }
            try
            {
                category.UpdatedBy = CurrentUserName; // Implement GetCurrentUserName()
                category.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM"); // Or your desired format

                // Save to Database
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) // Example of catching a database exception
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Error saving category to database",
                    Detail = "See inner exception for details." // (Log the actual ex.Message)
                };
                return StatusCode(500, problemDetails);
            }
            _logger.Information("Create building with ID {CategoryId} from database", category.CategoryId);

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest("Asset Type Id mismatch");
            }

            var categoryToUpdate = await _context.Categories.FindAsync(id);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            // Update properties from asset using a safe approach
            categoryToUpdate.CategoryDescription = category.CategoryDescription;
            categoryToUpdate.IsActive = category.IsActive;

            try
            {
                category.UpdatedBy = CurrentUserName;
                category.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BuildingExists(id))
            {
                return Conflict(new ProblemDetails { Title = "Conflict - Category has been modified" });
            }
            catch (DbUpdateException) // Consider more specific exception handling
            {
                // Log the error 
                return StatusCode(500, new ProblemDetails { Title = "Error updating category" });
            }

            bool BuildingExists(int id)
            {
                return _context.Categories.Any(e => e.CategoryId == id);
            }
            return NoContent(); // 204 Success, no content returned
        }

    }
}

