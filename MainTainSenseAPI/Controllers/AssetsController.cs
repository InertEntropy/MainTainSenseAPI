using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    [Authorize] // Assuming you want authorization for the AssetsController
    public class AssetsController : BaseController
    {
        public AssetsController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(context, logger, configuration) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return await _context.Assets
                .Include(a => a.AssetType)
                .Include(a => a.Location) 
                .ToListAsync(); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAssetById(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssetType)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null)
            {
                return NotFound();
            }

            return asset;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset(Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            try
            {
                asset.UpdatedBy = CurrrentUserName(HttpContext);
                asset.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");

                _context.Assets.Add(asset);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAssets", new { id = asset.AssetId }, asset);
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);
            }
        }

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


            assetToUpdate.AssetName = asset.AssetName;
            assetToUpdate.AssetDescription = asset.AssetDescription;
            assetToUpdate.AssetLocationId = asset.AssetLocationId;
            assetToUpdate.Assetstatus = asset.Assetstatus;
            assetToUpdate.Serialnumber = asset.Serialnumber;

            // Optional, if InstallDate can be changed:
            if (asset.InstallDate != null)
            {
                assetToUpdate.InstallDate = asset.InstallDate;
            }

            try
            {
                asset.UpdatedBy = CurrrentUserName(HttpContext);
                asset.LastUpdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss AM/PM");
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);
            }
        }
    }
}

