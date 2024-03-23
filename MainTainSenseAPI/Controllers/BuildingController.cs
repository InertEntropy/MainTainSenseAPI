using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MainTainSenseAPI.Controllers
{

    public class BuildingsController : BaseController
    {
        private readonly ApplicationDbContext _context; // Or your relevant data access mechanism

        public BuildingsController(ILogger<BuildingsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // 1. Get all buildings
        [HttpGet]
        public async Task<IActionResult> GetPagedBuildings(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            try
            {
                var buildingQuery = _context.Buildings.Select(b => new { b.Id, b.BuildingName, b.BuildingDescription });

                int totalCount = await _context.Buildings.CountAsync();
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var buildings = await buildingQuery
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
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex); // Handle database errors
            }
            catch (Exception ex)
            {
             
                // Log the exception for debugging
                logger.LogError(ex, "Error fetching paged buildings");
                return StatusCode(500, "A database error occurred.");
            
            }
        }

        // Get Building by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuilding(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Buidling not found");
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
                BuildingDescription = model.BuildingDescription
            };

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            logger.LogInformation("Building created: {Name}", building.BuildingName); // Log
            return CreatedAtAction(nameof(GetPagedBuildings), new { id = building.Id }, building);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuilding(int buildingId, [FromBody] BuildingViewModels model)
        {
            // 1. Find the existing building
            var building = await _context.Buildings.FindAsync(buildingId);
            if (building == null)
            {
                return NotFound("Building not found");
            }

            // 2. Input Validation (using ModelState)  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 3. Update properties
            building.BuildingName = model.BuildingName;
            building.BuildingDescription = model.BuildingDescription;
            logger.LogInformation("Building Updated: {Name}", building.BuildingName);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(building.Id); // Delegate to BaseController 
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
