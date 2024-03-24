using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationController(ILogger<LocationController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedLocation(int pageNumber = 1, int pageSize = 10,
                int? parentLocationId = null,
                int? buildingId = null,
                int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            var query = _context.Locations
                    .Include(a => a.Building)
                    .Include(a => a.ParentLocation);

            if (buildingId.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Location, Location?>)query.Where(a => a.BuildingId == buildingId.Value);
            }

            if (parentLocationId.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Location, Location?>)query.Where(a => a.ParentLocationId == parentLocationId.Value);
            }

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Location, Location?>)query.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            var locations = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

           return Ok(locations); 
        }

        // Get Location by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound("Location not found");
            }

            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = new Location
            {
                LocationName = model.LocationName,
                LocationDescription = model.LocationDescription,
                LocationPath = model.LocationPath,
                
                IsActive = model.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                logger.LogInformation("Location created with ID: {id}", location.Id);
                return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating location.");
                return StatusCode(500, "Error saving location to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(location.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            logger.LogError("Create Location: unhandled error occured");
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound("Location not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.Locations.FirstOrDefaultAsync(a => a.Id == id);

            if (entity == null)
            {
                return NotFound("Location not found");
            }

            entity.LocationName = model.LocationName;
            entity.LocationDescription = model.LocationDescription;
            entity.LastUpdated = DateTime.UtcNow;
            entity.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Location Updated: {LocationName}", entity.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating location.");
                return StatusCode(500, "Error saving updated location to database.");
            }

            return NoContent();
        }
    }
}