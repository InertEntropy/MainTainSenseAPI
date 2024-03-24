using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YesNo IsActive { get; private set; }

        public JobTitlesController(ILogger<JobTitlesController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            IQueryable<UserJobTitle> jobTitlesQuery = _context.UserJobTitles;

            if (isActive.HasValue)
            {
                bool isActiveBool = isActive.Value == 1; // Convert to boolean
                jobTitlesQuery = jobTitlesQuery.Where(a => a.IsActive == (isActiveBool ? YesNo.Yes : YesNo.No));
            }

            int totalCount = await _context.UserJobTitles.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var jobtitle = await jobTitlesQuery
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            var response = new PagedResponse<UserJobTitle>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = jobtitle
            };

            return Ok(response);
        }

        // GET: api/Asset/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobTitles(int id)
        {
            var assettype = await _context.UserJobTitles
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assettype == null)
            {
                return NotFound("JobTitles not found");
            }

            return Ok(assettype);
        }

        // POST: api/Asset
        [HttpPost]
        public async Task<IActionResult> CreateJobTitles(UserJobTitleViewModel jobTitlesViewModel)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Use your base controller's handler
            }
            // 1. Map ViewModel to JobTitles entity
            var jobTitles = new UserJobTitle
            {
                Name = jobTitlesViewModel.Name,
                IsActive = jobTitlesViewModel.IsActive,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                        ? CurrentUserName(_httpContextAccessor.HttpContext)
                        : "Unknown User"
            };
            try
            {
                _context.UserJobTitles.Add(jobTitles);
                await _context.SaveChangesAsync();

                logger.LogInformation("Job title created with ID: {id}", jobTitles.Id);
                return CreatedAtAction(nameof(GetJobTitles), new { id = jobTitles.Id }, jobTitles);
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while creating asset.");
                return StatusCode(500, "Error saving asset to database.");
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = HandleConcurrencyError(jobTitles.Id);
                if (result is ViewResult viewResult)
                {
                    viewResult.ViewData.Model = jobTitlesViewModel; // Pass the model back to the view
                    return viewResult;
                }
            }
            return StatusCode(500, "An unhandled error occurred.");
        }

        // PUT: api/Asset/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobTitles(int id, [FromBody] UserJobTitle model)
        {
            if (id != model.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            var jobTitles = await _context.UserJobTitles.FirstOrDefaultAsync(a => a.Id == id);

            if (jobTitles == null)
            {
                return NotFound("Job Title not found");
            }

            jobTitles.Name = model.Name;
            jobTitles.IsActive = model.IsActive;
            jobTitles.LastUpdated = DateTime.UtcNow;
            jobTitles.UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User";
            logger.LogInformation("Job title Updated: {id}", jobTitles.Id);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Database error while updating job title.");
                return StatusCode(500, "Error saving updated job title to database.");
            }

            return NoContent();
        }

    }
}
