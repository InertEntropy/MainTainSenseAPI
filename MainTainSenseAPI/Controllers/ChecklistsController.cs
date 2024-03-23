using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ChecklistsController(ILogger<ChecklistsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Checklists
        [HttpGet]
        public async Task<IActionResult> GetPagedChecklistss(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.Checklists.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var checklists = await _context.Checklists
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.ChecklistItems)
                .Include(a => a.TemplateTasks)
                .ToListAsync();

            var response = new PagedResponse<Checklist>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Checklist>)checklists
            };

            return Ok(response);
        }

        // GET: api/Checklists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChecklists(int id)
        {
            var checklist = await _context.Checklists
                .Include(a => a.ChecklistItems)
                .Include(a => a.TemplateTasks)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (checklist == null)
            {
                return NotFound("Checklists not found");
            }

            return Ok(checklist);
        }

        // POST: api/Checklists
        [HttpPost]
        public async Task<IActionResult> CreateChecklists([FromBody] Checklist checklist)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChecklists), new { id = checklist.Id }, checklist);
        }

        // PUT: api/Checklists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChecklists(int id, [FromBody] Checklist checklist)
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

            logger.LogInformation("Checklists Updated: {Name}", checklist.ChecklistName);
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
