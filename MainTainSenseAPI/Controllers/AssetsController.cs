using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using MainTainSenseAPI.Models;      
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Core;

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

        // GET: api/Assets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return await _context.Assets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAssetById(int id)
        {
            var asset = await _context.Assets
                                      .Include(a => a.AssetType)
                                      .FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null)
            {
                return NotFound();
            }

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
                            errorsDict = new Dictionary<string, string[]>();
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
            _logger.Information("Create asset with ID {AssetId} from database", asset.AssetId);


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
            assetToUpdate.AssetLocation = asset.AssetLocation;
            assetToUpdate.Assetstatus = asset.Assetstatus;
           
            try
            {
                asset.UpdatedBy = CurrentUserName; 
                asset.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM"); 
                await _context.SaveChangesAsync();
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
