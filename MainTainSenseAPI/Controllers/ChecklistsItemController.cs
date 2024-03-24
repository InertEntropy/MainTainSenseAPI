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
    public class ChecklistsItemController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChecklistsItemController(ILogger<ChecklistsItemController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedChecklistsItem(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<ChecklistItem> query = _context.ChecklistItems;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                query = query.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            int totalCount = await _context.ChecklistItems.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var checklistitems = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<ChecklistItem>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<ChecklistItem>)checklistitems,
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChecklistsItem(int id)
        {
            var category = await _context.ChecklistItems.FindAsync(id);

            if (category == null)
            {
                return NotFound("Checklist Items not found");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChecklistsItem([FromBody] ChecklistItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checklistitems = new ChecklistItem
            {
                ChecklistItemsDescription = model.ChecklistItemsDescription,
                IsCompleted = model.IsCompleted,
                SortOrder = model.SortOrder,
                IsActive = YesNo.Yes,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.ChecklistItems.Add(checklistitems);
                await _context.SaveChangesAsync();

                logger.LogInformation("ChecklistsItem created with ID: {checklistsItemsId}", checklistitems.Id);
                return CreatedAtAction(nameof(GetChecklistsItem), new { id = checklistitems.Id }, checklistitems);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating Check list item.");
                return StatusCode(500, "Error saving Check list items to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(checklistitems.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChecklistsItem(int id, [FromBody] ChecklistItemViewModel model)
        {

            var checklists = await _context.ChecklistItems.FindAsync(id);
            if (checklists == null)
            {
                return NotFound("ChecklistsItem not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            checklists.ChecklistItemsDescription = model.ChecklistItemsDescription;
            checklists.IsActive = model.IsActive;
            checklists.LastUpdated = DateTime.UtcNow;
            checklists.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Checklist Items Updated: {id}", checklists.ChecklistItemsDescription);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating checklist items.");
                return StatusCode(500, "Error saving updated checklist items to database.");
            }

            return NoContent();
        }
    }
}