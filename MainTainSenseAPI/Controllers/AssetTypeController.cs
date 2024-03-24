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
    public class AssetTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YesNo IsActive { get; private set; }

        public AssetTypeController(ILogger<AssetTypeController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Asset
        [HttpGet]
        public async Task<IActionResult> GetPagedAssets(int pageNumber = 1, int pageSize = 10, int? isActive = null)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            IQueryable<AssetType> assetTypesQuery = _context.AssetTypes;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1; // Convert to boolean
                assetTypesQuery = assetTypesQuery.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            int totalCount = await _context.AssetTypes.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var assettypes = await assetTypesQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            var response = new PagedResponse<AssetType>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = assettypes
            };

            return Ok(response);
        }

        // GET: api/Asset/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetType(int id)
        {
            var assettype = await _context.AssetTypes
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assettype == null)
            {
                return NotFound("AssetType not found");
            }

            return Ok(assettype);
        }

        // POST: api/Asset
        [HttpPost]
        public async Task<IActionResult> CreateAssetType(AssetTypeViewModel assetTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Use your base controller's handler
            }
            // 1. Map ViewModel to AssetType entity
            var assetType = new AssetType
            {
                AssetTypeName = assetTypeViewModel.AssetTypeName,
                AssetTypeDescription = assetTypeViewModel.AssetTypeDescription,
                IsActive = assetTypeViewModel.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.AssetTypes.Add(assetType);
                await _context.SaveChangesAsync();

                logger.LogInformation("Asset type created with ID: {assettypeId}", assetType.Id);
                return CreatedAtAction(nameof(GetAssetType), new { id = assetType.Id }, assetType);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating asset.");
                return StatusCode(500, "Error saving asset to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(assetType.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = assetTypeViewModel; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        // PUT: api/Asset/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssetType(int id, [FromBody] AssetType model)
        {
            if (id != model.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            var assetType = await _context.AssetTypes.FirstOrDefaultAsync(a => a.Id == id);

            if (assetType == null)
            {
                return NotFound("Asset type not found");
            }

            assetType.AssetTypeDescription = model.AssetTypeDescription;
            assetType.IsActive = model.IsActive;
            assetType.LastUpdated = DateTime.UtcNow;
            assetType.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Asset type Updated: {id}", assetType.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating asset type.");
                return StatusCode(500, "Error saving updated asset type to database.");
            }

            return NoContent();
        }

    }
}
