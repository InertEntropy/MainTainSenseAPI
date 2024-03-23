using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ILogger<LocationController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<IActionResult> GetPagedLocations(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.Locations.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var locations = await _context.Locations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.ChildLocations)
                .ToListAsync();

            var response = new PagedResponse<Location>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Location>)locations
            };

            return Ok(response);
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _context.Locations
                .Include(a => a.ChildLocations)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (location == null)
            {
                return NotFound("Location not found");
            }

            return Ok(location);
        }

        // POST: api/Location
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location location)
        {
            if (id != location.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(location).State = EntityState.Modified;

            logger.LogInformation("Location Updated: {Name}", location.LocationName);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(location.Id); // Delegate to BaseController 
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
