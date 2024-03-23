using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{

    public class CatecoryController : BaseController
    {
        private readonly ApplicationDbContext _context; // Or your relevant data access mechanism

        public CatecoryController(ILogger<CatecoryController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // 1. Get all categories
        [HttpGet("api/category")]
        public async Task<IActionResult> GetPagedCategory(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            try
            {
                int totalCount = await _context.Categories.CountAsync();
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var categories = await _context.Categories
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

                var response = new PagedResponse<Category>
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Items = categories
                };

                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex); // Handle database errors
            }
            catch (Exception ex)
            {

                // Log the exception for debugging
                logger.LogError(ex, "Error fetching paged category");
                return StatusCode(500, "A database error occurred.");
            }
        }
      
        // Get Category by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category);
        }

        // Update Category 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryViewModel model)
        {
            // 1. Find the existing category
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            // 2. Input Validation 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 3. Update properties
            category.CategoryDescription = model.CategoryDescription;

            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id)) // Check if the category was deleted
                {
                    return NotFound();
                }
                else
                {
                    return HandleConcurrencyError(category.Id); // Delegate to BaseController
                }
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);  // Pass the exception 
            }

            return NoContent(); // Indicate successful update
        }

        // Helper method
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }

}
