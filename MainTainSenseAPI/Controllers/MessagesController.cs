using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ILogger<MessageController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
        }

        // GET: api/Message
        [HttpGet]
        public async Task<IActionResult> GetPagedMessages(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid page number or page size");
            }

            int totalCount = await _context.Messages.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var messages = await _context.Messages
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.InverseParenttMessage)
                .ToListAsync();

            var response = new PagedResponse<Message>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = (IEnumerable<Message>)messages
            };

            return Ok(response);
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _context.Messages
                .Include(a => a.InverseParenttMessage)

                .FirstOrDefaultAsync(a => a.Id == id);

            if (message == null)
            {
                return NotFound("Message not found");
            }

            return Ok(message);
        }

        // POST: api/Message
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState); // Handle validation errors in BaseController
            }

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        // PUT: api/Message/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message message)
        {
            if (id != message.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return HandleValidationErrors(ModelState);
            }

            _context.Entry(message).State = EntityState.Modified;

            logger.LogInformation("Message Updated: {Name}", message.Subject);
            // 4. Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return HandleConcurrencyError(message.Id); // Delegate to BaseController 
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
