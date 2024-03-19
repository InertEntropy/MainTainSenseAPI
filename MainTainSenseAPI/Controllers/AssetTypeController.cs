using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AssetTypeController(ILogger<AssetController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Asset
        [HttpGet]
        public async Task<IActionResult> GetPagedAssets(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.AssetTypes.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var assettypes = await _context.AssetTypes
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
        public async Task<IActionResult> CreateAssetType([FromBody] AssetType assettype)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.AssetTypes.Add(assettype);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssetType), new { id = assettype.Id }, assettype);
        }

        // PUT: api/Asset/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssetType(int id, [FromBody] AssetType assettype)
        {
            if (id != assettype.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(assettype).State = EntityState.Modified;

            logger.LogInformation("Asset Updated: {Name}", assettype.AssetTypeName);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(assettype.Id); // Delegate to BaseController 
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
