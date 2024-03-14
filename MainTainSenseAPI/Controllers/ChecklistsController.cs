using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainTainSenseAPI.Models;

namespace MainTainSenseAPI.Controllers
{
    public class ChecklistsController : BaseController
    {
        private readonly MainTainSenseDataContext _context;

        public ChecklistsController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetChecklists()
        {
            return await _context.Checklists
                                 .Include(c => c.ChecklistItems) // Include related data if desired
                                 .ToListAsync();
        }
        // GET: api/Checklists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Checklist>> GetChecklist(int id)
        {
            var checklist = await _context.Checklists
                                          .Include(c => c.ChecklistItems) // Include related data
                                          .FirstOrDefaultAsync(c => c.ChecklistId == id);

            if (checklist == null)
            {
                return NotFound();
            }

            return checklist;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChecklist(int id, Checklist checklist)
        {
            if (id != checklist.ChecklistId)
            {
                return BadRequest("Checklist ID mismatch."); // Ensure IDs match
            }

            // Option 1: Simple Update (With caution)
            _context.Entry(checklist).State = EntityState.Modified;

            // Option 2: Selective Update (More control)
            var existingChecklist = await _context.Checklists.FindAsync(id);
            if (existingChecklist == null)
            {
                return NotFound();
            }

            // Update individual properties as needed
            existingChecklist.ChecklistName = checklist.ChecklistName;
            existingChecklist.IsActive = checklist.IsActive;
            // ... update other properties

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Or re-throw a different exception with more details
                }
            }

            return NoContent(); // Standard success response for PUT
        }

        // Helper method for checking if a checklist exists
        private bool ChecklistExists(int id)
        {
            return _context.Checklists.Any(e => e.ChecklistId == id);
        }

        [HttpPost]
        public async Task<ActionResult<Checklist>> PostChecklist(Checklist checklist)
        {
            // Validation (apply data annotations to your Checklist model or add checks here)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            _logger.Information("Create asset with ID {AssetId} from database", checklist.ChecklistId);

            // For best practices, return CreatedAtAction. 
            return CreatedAtAction("GetChecklist", new { id = checklist.ChecklistId }, checklist);
        }
    }
}
