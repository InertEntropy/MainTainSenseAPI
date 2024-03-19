using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AssetController(ILogger<AssetController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            int totalCount = await _context.Assets.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var assets = await _context.Assets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.AssetType)
                .Include(a => a.Location)
                .Include(a => a.Assetstatus)
                .ToListAsync();

            var response = new PagedResponse<Asset>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = assets
            };

            return Ok(response);
        }

        // GET: api/Asset/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsset(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssetType)
                .Include(a => a.Location)
                .Include(a => a.Assetstatus)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
            {
                return NotFound("Asset not found");
            }

            return Ok(asset);
        }

        // POST: api/Asset
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
        }

        // PUT: api/Asset/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] Asset asset)
        {
            if (id != asset.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(asset).State = EntityState.Modified;

            logger.LogInformation("Asset Updated: {Name}", asset.AssetName);
            // 4. Save changes
            try
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(asset.Id); // Delegate to BaseController 
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
