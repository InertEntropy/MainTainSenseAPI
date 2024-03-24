using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{

    public class CatecoryController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CatecoryController(ILogger<CatecoryController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedCategory(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Category> query = _context.Categories;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                query = query.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No)); 
            }

            int totalCount = await _context.Categories.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var categories = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<Category> 
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Category>)categories,
            };

            return Ok(response);
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categories = new Category
            {
                CategoryDescription = model.CategoryDescription,
                IsActive = YesNo.Yes,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Categories.Add(categories);
                await _context.SaveChangesAsync();

                logger.LogInformation("Category created with ID: {categoriesId}", categories.Id);
                return CreatedAtAction(nameof(GetCategory), new { id = categories.Id }, categories);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating category.");
                return StatusCode(500, "Error saving category to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(categories.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryViewModel model)
        {

            var categories = await _context.Categories.FindAsync(categoryId);
            if (categories == null)
            {
                return NotFound("Category not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(categories).State = EntityState.Modified;

            logger.LogInformation("Category Updated: {id}", categories.CategoryDescription);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating category.");
                return StatusCode(500, "Error saving updated category to database.");
            }

            return NoContent();
        }
    }
}
