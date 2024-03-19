using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public JobTitlesController(ILogger<JobTitlesController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/JobTitles
        [HttpGet]
        public async Task<IActionResult> GetJobTitles()
        {
            var jobTitles = await _context.UserJobTitles
                                    .Select(j => new { j.Id, j.Name }) // Select if needed
                                    .ToListAsync();
            return Ok(jobTitles);
        }

        // GET: api/JobTitles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobTitle(int id)
        {
            var jobTitle = await _context.UserJobTitles.FindAsync(id);

            if (jobTitle == null)
            {
                return NotFound("Job Title not found");
            }

            return Ok(jobTitle);
        }

        // POST: api/JobTitles
        [HttpPost]
        public async Task<IActionResult> CreateJobTitle([FromBody] UserJobTitle model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserJobTitles.Add(model);
            await _context.SaveChangesAsync();

            logger.LogInformation("Job Title created: {Name}", model.Name);
            return CreatedAtAction(nameof(GetJobTitles), new { id = model.Id }, model);
        }

        // PUT: api/JobTitles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobTitle(int id, [FromBody] UserJobTitle model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(model).State = EntityState.Modified;
            logger.LogInformation("Job Title Updated: {Name}", model.Name);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(model.Id);
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);
            }

            return NoContent();
        }
    }
}

