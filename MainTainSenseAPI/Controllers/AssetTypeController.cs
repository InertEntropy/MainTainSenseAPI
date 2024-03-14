using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization; // Add if using Authorize

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypesController : BaseController
    {
        private readonly MainTainSenseDataContext _context;

        public AssetTypesController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetType>>> GetAssetTypes()
        {
            return await _context.AssetTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetType>> GetAssetType(int id)
        {
            var assetType = await _context.AssetTypes.FindAsync(id);
            if (assetType == null) { return NotFound(); }
            return assetType;
        }

        [Authorize] 
        [HttpPost]
        public async Task<ActionResult<AssetType>> CreateAssetType(AssetType assetType)
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
                assetType.UpdatedBy = CurrentUserName; // Implement GetCurrentUserName()
                assetType.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM"); // Or your desired format

                // Save to Database
                _context.AssetTypes.Add(assetType);
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
            _logger.Information("Create asset type with ID {AssetTypeId} from database", assetType.AssetTypeId);

            return CreatedAtAction("GetAssetType", new { id = assetType.AssetTypeId }, assetType);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssetType(int id, AssetType assetType)
        {
            if (id != assetType.AssetTypeId)
            {
                return BadRequest("Asset Type Id mismatch");
            }

            var assetTypeToUpdate = await _context.AssetTypes.FindAsync(id);
            if (assetTypeToUpdate == null)
            {
                return NotFound();
            }

            // Update properties from asset using a safe approach
            assetTypeToUpdate.AssetTypeName = assetType.AssetTypeName;
            assetTypeToUpdate.AssetTypeDescription = assetType.AssetTypeDescription;
            assetTypeToUpdate.Active = assetType.Active;

            try
            {
                assetType.UpdatedBy = CurrentUserName;
                assetType.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AssetTypeExists(id))
            {
                return Conflict(new ProblemDetails { Title = "Conflict - Asset type has been modified" });
            }
            catch (DbUpdateException) // Consider more specific exception handling
            {
                // Log the error 
                return StatusCode(500, new ProblemDetails { Title = "Error updating asset type" });
            }

            bool AssetTypeExists(int id)
            {
                return _context.Assets.Any(e => e.AssetTypeId == id);
            }
            return NoContent(); // 204 Success, no content returned
        }


    }
}
