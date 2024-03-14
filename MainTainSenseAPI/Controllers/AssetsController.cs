using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")] // Routing pattern
    [ApiController]
    public class AssetsController : BaseController
    {
        private readonly MainTainSenseDataContext _context;

        public AssetsController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
       : base(logger, configuration)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets(
            int? assetTypeId,
            AssetStatus? assetStatus)
        {
            var query = _context.Assets.AsQueryable();

            if (assetTypeId.HasValue)
            {
                query = query.Where(c => c.AssetTypeId == assetTypeId.Value);
            }

            if (assetStatus.HasValue)
            {
                query = query.Where(c => c.Assetstatus == assetStatus.Value);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAssetById(
            int id,
            int? assetTypeId,
             AssetStatus? assetStatus)
        {
            var query = _context.Assets.AsQueryable();

            if (assetTypeId.HasValue)
            {
                query = query.Where(a => a.AssetTypeId == assetTypeId);
            }

            if (assetStatus.HasValue)
            {
                query = query.Where(a => a.Assetstatus == assetStatus);
            }

            var asset = await query
                                .Include(a => a.AssetType)
                                .FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null) { return NotFound(); }
            return asset;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Asset>> CreateAsset(Asset asset)
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
                asset.UpdatedBy = CurrentUserName; // Implement GetCurrentUserName()
                asset.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM"); // Or your desired format

                // Save to Database
                _context.Assets.Add(asset);
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
            
            await _context.SaveChangesAsync();

            _logger.Information("Create asset with ID {AssetId} from database", asset.AssetId);

            if (asset.AssetType.IsMachine == YesNo.Yes)
            {
                var newLocation = new Location
                {
                    LocationName = asset.AssetName,
                    LocationDescription = asset.AssetDescription,
                };
                
                _context.Locations.Add(newLocation);
            }

            await _context.SaveChangesAsync();
            // (4) Success Response (Best Practice)
            return CreatedAtAction("GetAssets", new { id = asset.AssetId }, asset);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, Asset asset)
        {
            if (id != asset.AssetId)
            {
                return BadRequest("Asset ID mismatch");
            }

            var assetToUpdate = await _context.Assets.FindAsync(id);
            if (assetToUpdate == null)
            {
                return NotFound();
            }

            // Update properties from asset using a safe approach
            assetToUpdate.AssetName = asset.AssetName;
            assetToUpdate.AssetLocationId = asset.AssetLocationId;
            assetToUpdate.Assetstatus = asset.Assetstatus;
           
            try
            {
                asset.UpdatedBy = CurrentUserName; 
                asset.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqliteException sqliteEx)
                    {
                        switch (sqliteEx.ErrorCode)
                        {
                            case 19:  // Example: SQLite Constraint Violation
                                return BadRequest("Cannot update due to related data");

                            case 1:  // Database locked
                                return StatusCode(503, "Database temporarily busy, please try again");

                            case 6:  //  Cannot open
                                _logger.Error(sqliteEx, "Failed to open database");
                                return StatusCode(500, "Database error, contact administrator");

                            case 11:  // Database is corrupt
                                _logger.Fatal(sqliteEx, "Database corruption detected. Application terminating."); // Use LogFatal for critical errors
                                return StatusCode(500, "Critical database failure, contact support.");

                            default:
                                _logger.Error(ex, "SQLite Error");
                                return StatusCode(500);
                        }
                    }
                    else
                    {
                        // Log non-SQL exception
                        return StatusCode(500);
                    }
                }
            }

            catch (DbUpdateConcurrencyException) when (!AssetExists(id))
            {
                return Conflict(new ProblemDetails { Title = "Conflict - Asset has been modified" });
            }
            catch (DbUpdateException) // Consider more specific exception handling
            {
                // Log the error 
                return StatusCode(500, new ProblemDetails { Title = "Error updating asset" });
            }
            
            bool AssetExists(int id)
            {
                return _context.Assets.Any(e => e.AssetId == id);
            }
            return NoContent(); // 204 Success, no content returned
        }
    }
}
