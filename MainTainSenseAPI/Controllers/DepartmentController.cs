using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ILogger<DepartmentsController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                                    .Select(d => new { d.Id, d.Name }) // Select if needed
                                    .ToListAsync();
            return Ok(departments);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound("Department not found");
            }

            return Ok(department);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Departments.Add(model);
            await _context.SaveChangesAsync();

            logger.LogInformation("Department created: {Name}", model.Name);
            return CreatedAtAction(nameof(GetDepartments), new { id = model.Id }, model);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(model).State = EntityState.Modified;
            logger.LogInformation("Department updated: {Name}", model.Name);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(model.Id);
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseException(ex);
            }

            return NoContent();
        }
    }
}


