using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{

    public class BuildingsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuildingsController(ILogger<BuildingsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // 1. Get all buildings
        [HttpGet]
        public async Task<IActionResult> GetPagedBuildings(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<Building> buildingsQuery = _context.Buildings;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1;
                buildingsQuery = buildingsQuery.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No)); // **Removed assignment** 
            }

            int totalCount = await _context.Buildings.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var buildings = await buildingsQuery
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            var response = new PagedResponse<Building> // Assuming you have a 'Building' entity
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Building>)buildings
            };

            return Ok(response);
        }


        // Get Building by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuilding(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Building not found");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBuilding([FromBody] BuildingViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var building = new Building
            {
                BuildingName = model.BuildingName,
                BuildingDescription = model.BuildingDescription,
                IsActive = model.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.Buildings.Add(building);
                await _context.SaveChangesAsync();

                logger.LogInformation("Building created with ID: {buildingId}", building.Id);
                return CreatedAtAction(nameof(GetBuilding), new { id = building.Id }, building);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating building.");
                return StatusCode(500, "Error saving building to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(building.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = model; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuilding(int id, [FromBody] BuildingViewModels model)
        {

            if (id != model.Id)
            {
                return NotFound("Building not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buildings = await _context.Buildings.FirstOrDefaultAsync(a => a.Id == id);

            if (buildings == null)
            {
                return NotFound("Building not found");
            }

            buildings.BuildingDescription = model.BuildingDescription;
            buildings.BuildingName = model.BuildingName;
            buildings.IsActive = model.IsActive;
            buildings.LastUpdated = DateTime.UtcNow;
            buildings.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Building Updated: {BuildingName}", buildings.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating building.");
                return StatusCode(500, "Error saving updated building to database.");
            }

            return NoContent();
        }
    }
}

