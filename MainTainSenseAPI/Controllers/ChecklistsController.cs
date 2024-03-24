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
    public class ChecklistsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChecklistsController(ILogger<ChecklistsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedChecklists(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Checklist> query = _context.Checklists;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                query = query.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            int totalCount = await _context.Checklists.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var checklists = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<Checklist>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Checklist>)checklists,
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChecklists(int id)
        {
            var category = await _context.Checklists.FindAsync(id);

            if (category == null)
            {
                return NotFound("Checklist not found");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChecklists([FromBody] ChecklistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checklists = new Checklist
            {
                ChecklistName = model.ChecklistName,
                IsActive = YesNo.Yes,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Checklists.Add(checklists);
                await _context.SaveChangesAsync();

                logger.LogInformation("Checklists created with ID: {checklistsId}", checklists.Id);
                return CreatedAtAction(nameof(GetChecklists), new { id = checklists.Id }, checklists);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating checklists.");
                return StatusCode(500, "Error saving category to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(checklists.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChecklists(int id, [FromBody] ChecklistViewModel model)
        {

            var checklists = await _context.Checklists.FindAsync(id);
            if (checklists == null)
            {
                return NotFound("Checklists not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            checklists.ChecklistName = model.ChecklistName;
            checklists.IsActive = model.IsActive;
            checklists.LastUpdated = DateTime.UtcNow;
            checklists.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Checklist Updated: {id}", checklists.ChecklistName);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating checklist.");
                return StatusCode(500, "Error saving updated checklist to database.");
            }

            return NoContent();
        }
    }
}
