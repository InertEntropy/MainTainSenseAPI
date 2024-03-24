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
    public class FrequencyController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FrequencyController(ILogger<FrequencyController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedFrequency(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Frequency> frequenciesQuery = _context.Frequencies;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                frequenciesQuery = frequenciesQuery.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No)); // **Removed assignment** 
            }

            int totalCount = await _context.Frequencies.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var frequencies = await frequenciesQuery
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<Frequency> // Assuming you have a 'Frequency' entity
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Frequency>)frequencies
            };

            return Ok(response);
        }


        // Get Frequency by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFrequency(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Frequency not found");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFrequency([FromBody] FrequencyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var frequency = new Frequency
            {
                FrequencyDescription = model.FrequencyDescription,
                IntervalValue = model.IntervalValue,
                TimeUnit = model.TimeUnit,
                DayofMonth = model.DayofMonth,
                Dayofweek = model.Dayofweek,
                FrequencyMonth = model.FrequencyMonth,
                IsActive = model.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Frequencies.Add(frequency);
                await _context.SaveChangesAsync();

                logger.LogInformation("Frequency created with ID: {Id}", frequency.Id);
                return CreatedAtAction(nameof(GetFrequency), new { id = frequency.Id }, frequency);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating frequency.");
                return StatusCode(500, "Error saving frequency to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(frequency.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFrequency(int id, [FromBody] FrequencyViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound("Frequency not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var frequencies = await _context.Frequencies.FirstOrDefaultAsync(a => a.Id == id);

            if (frequencies == null)
            {
                return NotFound("Frequency not found");
            }

            frequencies.FrequencyDescription = model.FrequencyDescription;
            frequencies.IntervalValue = model.IntervalValue;
            frequencies.DayofMonth = model.DayofMonth;
            frequencies.Dayofweek = model.Dayofweek;
            frequencies.FrequencyMonth = model.FrequencyMonth;
            frequencies.TimeUnit = model.TimeUnit;
            frequencies.IsActive = model.IsActive;
            frequencies.LastUpdated = DateTime.UtcNow;
            frequencies.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Frequency Updated: {FrequencyName}", frequencies.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating building.");
                return StatusCode(500, "Error saving updated frequency to database.");
            }

            return NoContent();
        }
    }
}