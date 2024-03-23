using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public FrequencyController(ILogger<FrequencyController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Frequency
        [HttpGet]
        public async Task<IActionResult> GetPagedFrequency(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.Frequencies.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var frequencys = await _context.Frequencies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.TemplateTasks)
                .ToListAsync();

            var response = new PagedResponse<Frequency>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Frequency>)frequencys
            };

            return Ok(response);
        }

        // GET: api/Frequency/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFrequency(int id)
        {
            var frequencys = await _context.Frequencies
                .Include(a => a.TemplateTasks)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (frequencys == null)
            {
                return NotFound("Frequency not found");
            }

            return Ok(frequencys);
        }

        // POST: api/Frequency
        [HttpPost]
        public async Task<IActionResult> CreateFrequency([FromBody] Frequency frequency)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.Frequencies.Add(frequency);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFrequency), new { id = frequency.Id }, frequency);
        }

        // PUT: api/Frequency/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFrequency(int id, [FromBody] Frequency frequency)
        {
            if (id != frequency.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(frequency).State = EntityState.Modified;

            logger.LogInformation("Frequency Updated: {Name}", frequency.FrequencyDescription);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(frequency.Id); // Delegate to BaseController 
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
