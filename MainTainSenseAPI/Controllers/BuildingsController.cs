using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization; // Add if using Authorize

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : BaseController
    {
        private readonly MainTainSenseDataContext _context;

        public BuildingsController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildings()
        {
            return await _context.Buildings
                                  .Include(b => b.Locations)
                                  .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null) { return NotFound(); }
            return building;
        }

        [Authorize] // Assuming you want creation to be authorized
        [HttpPost]
        public async Task<ActionResult<Building>> CreateBuilding(Building building)
        {
            if (!ModelState.IsValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "One or more validation errors occurred.",
                    Detail = "Please check the following fields:",
                };

                problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();

                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        // Check if errorsDict is null before using it
                        if (problemDetails.Extensions["errors"] is not Dictionary<string, string[]> errorsDict)
                        {
                            errorsDict = [];
                            problemDetails.Extensions["errors"] = errorsDict;
                        }

                        errorsDict.Add(entry.Key, entry.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    }
                }
                return BadRequest(problemDetails); // Returns custom error response
            }
            try
            {
                building.UpdatedBy = CurrentUserName; // Implement GetCurrentUserName()
                building.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM"); // Or your desired format

                // Save to Database
                _context.Buildings.Add(building);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) // Example of catching a database exception
            {
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Error saving asset to database",
                    Detail = "See inner exception for details." // (Log the actual ex.Message)
                };
                return StatusCode(500, problemDetails);
            }
            _logger.Information("Create building with ID {BuildingId} from database", building.BuildingId);

            return CreatedAtAction("GetBuilding", new { id = building.BuildingId }, building);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuilding(int id, Building building)
        {
            if (id != building.BuildingId)
            {
                return BadRequest("Building Id mismatch");
            }

            var buildingToUpdate = await _context.Buildings.FindAsync(id);
                if (buildingToUpdate == null)
                {
                    return NotFound();
                }

                buildingToUpdate.BuildingName = building.BuildingName;
                buildingToUpdate.BuildingDescription = building.BuildingDescription;
                buildingToUpdate.IsActive = building.IsActive;

                try
                {
                    building.UpdatedBy = CurrentUserName;
                    building.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!BuildingExists(id))
                {
                    return Conflict(new ProblemDetails { Title = "Conflict - Building has been modified" });
                }
                catch (DbUpdateException) // Consider more specific exception handling
                {
                    // Log the error 
                    return StatusCode(500, new ProblemDetails { Title = "Error updating building" });
                }

                bool BuildingExists(int id)
                {
                    return _context.Buildings.Any(e => e.BuildingId == id);
                }
                return NoContent(); // 204 Success, no content returned
        }
    }
}


