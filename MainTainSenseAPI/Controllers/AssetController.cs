using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common; // For ValidationAttributes

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YesNo IsActive { get; private set; }

        public AssetsController(ILogger<AssetsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Assets 
        [HttpGet]
        public async Task<ActionResult<PagedResponse<Asset>>> GetAssets(
            int pageNumber = 1,
            int pageSize = 10,
            int? assetTypeId = null,
            int? locationId = null,
            int? assetStatusId = null,
            int? isActive = null
        )

        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            var query = _context.Assets
                     .Include(a => a.AssetType)
                     .Include(a => a.Location)
                     .Include(a => a.Assetstatus);

            if (assetTypeId.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Asset, AssetStatus>)query.Where(a => a.AssetTypeId == assetTypeId);
            }

            if (locationId.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Asset, AssetStatus>)query.Where(a => a.AssetLocationId == locationId);
            }

            if (assetStatusId.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Asset, AssetStatus>)query.Where(a => a.Assetstatus == (AssetStatus)assetStatusId);
            }

            if (isActive.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Asset, AssetStatus>)query.Where(a => a.IsActive == (isActive.Value == 1 ? YesNo.Yes : YesNo.No));
            }

            var assets = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return Ok(assets);
        }

        // GET: api/Assets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var asset = await _context.Assets
                                      .Include(a => a.AssetType)
                                      .Include(a => a.Location)
                                      .Include(a => a.Assetstatus)
                                      .Include(a => a.IsActive)
                                      .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
            {
                return NotFound("Asset not found");
            }

            return Ok(asset);
        }

        // POST: api/Assets
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] AssetViewModels assetViewModel)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            var assetStatus = (AssetStatus)Enum.Parse(typeof(AssetStatus), assetViewModel.AssetStatusId.ToString());
            var assetType = await _context.AssetTypes.FindAsync(assetViewModel.AssetTypeId);
            var assetLocation = await _context.Locations.FindAsync(assetViewModel.AssetLocationId);
            var asset = new Asset
            {
                AssetType = assetType, 
                AssetName = assetViewModel.AssetName,
                Serialnumber = assetViewModel.Serialnumber,
                Location = assetLocation,
                Assetstatus = assetStatus,
                AssetDescription = assetViewModel.AssetDescription,
                InstallDate = assetViewModel.InstallDate,

                IsActive = YesNo.Yes, 
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            :"Unknown User"

            };
            try
            {
                _context.Assets.Add(asset);
                await _context.SaveChangesAsync();

                logger.LogInformation("Asset created with ID: {AssetName}", asset.Id);
                return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating asset.");
                return StatusCode(500, "Error saving asset to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(asset.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = assetViewModel; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }
    

        // PUT: api/Assets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] AssetViewModels assetViewModel)
        {
            if (id != assetViewModel.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                // Repopulate dropdown lists
                assetViewModel.AvailableAssetTypes = await _context.AssetTypes
                                                          .Select(at => new Models.Views.SelectListItem
                                                          {
                                                              Value = at.Id,
                                                              Text = at.AssetTypeName
                                                          })
                                                          .ToListAsync();

                assetViewModel.AvailableLocations = await _context.Locations
                                                          .Select(at => new Models.Views.SelectListItem
                                                          {
                                                              Value = at.Id,
                                                              Text = at.LocationName
                                                          })
                                                          .ToListAsync();

                assetViewModel.AvailableAssetStatuses = Enum.GetValues<AssetStatus>()
                                              .Select(status => new Models.Views.SelectListItem
                                              {
                                                  Value = ((int)status),
                                                  Text = status.ToString()
                                              })
                                              .ToList(); 

                return BadRequest(ModelState);
            }

            // Fetch existing asset
            var asset = await _context.Assets
                                      .Include(a => a.AssetType)
                                      .Include(a => a.Location)
                                      .Include(a => a.Assetstatus)
                                      .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
            {
                return NotFound("Asset not found");
            }
            var assetStatus = (AssetStatus)Enum.Parse(typeof(AssetStatus), assetViewModel.AssetStatusId.ToString());
            var assetType = await _context.AssetTypes.FindAsync(assetViewModel.AssetTypeId);
            var assetLocation = await _context.Locations.FindAsync(assetViewModel.AssetLocationId);

            asset.AssetType = assetType;
            asset.Location = assetLocation;
            asset.Assetstatus = assetStatus;
            asset.AssetName = assetViewModel.AssetName;
            asset.Serialnumber = assetViewModel.Serialnumber;
            asset.AssetDescription = assetViewModel.AssetDescription;
            asset.InstallDate = assetViewModel.InstallDate;
            asset.IsActive = IsActive;
            asset.LastUpdated = DateTime.UtcNow;
            asset.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating asset.");
                return StatusCode(500, "Error saving updated asset to database.");
            }

            return NoContent();
        }

        // DELETE: api/Assets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound("Asset not found");
            }

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

