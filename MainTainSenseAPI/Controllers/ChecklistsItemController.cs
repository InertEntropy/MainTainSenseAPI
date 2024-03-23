using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistItemController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ChecklistItemController(ILogger<ChecklistItemController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/ChecklistItem
        [HttpGet]
        public async Task<IActionResult> GetPagedChecklistItems(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.ChecklistItems.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var checklistquery = await _context.ChecklistItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PagedResponse<Checklist>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Checklist>)checklistquery
            };

            return Ok(response);
        }

        // GET: api/ChecklistItem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChecklistItem(int id)
        {
            var checklistquery = await _context.ChecklistItems
                .FirstOrDefaultAsync(a => a.Id == id);

            if (checklistquery == null)
            {
                return NotFound("ChecklistItem not found");
            }

            return Ok(checklistquery);
        }

        // POST: api/ChecklistItem
        [HttpPost]
        public async Task<IActionResult> CreateChecklistItem([FromBody] ChecklistItem model)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.ChecklistItems.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChecklistItem), new { id = model.Id }, model);
        }

        // PUT: api/ChecklistItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChecklistItem(int id, [FromBody] ChecklistItem checklist)
        {
            if (id != checklist.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(checklist).State = EntityState.Modified;

            logger.LogInformation("ChecklistItem Updated: {Name}", checklist.ChecklistItemsDescription);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(checklist.Id); // Delegate to BaseController 
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
