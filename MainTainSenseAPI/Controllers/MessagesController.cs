using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MessagesController(ILogger<MessagesController> logger, IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(
            int pageNumber = 1,
            int pageSize = 10,
            bool? isRead = null)
        {
            var currentUserName = CurrentUserName(_httpContextAccessor.HttpContext); 
            if (currentUserName == null)
            {
                logger.LogError("Unable to determine current user after caching");
                return Problem("Internal server error.", statusCode: 500);
            }

            var messagesQuery = _context.Messages
                                .Where(m => m.RecipientId == CurrentUserName(_httpContextAccessor.HttpContext))
                                .Include(m => m.Sender)
                                .Include(m => m.Recipient);

            if (isRead.HasValue)
            {
                messagesQuery = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Message, ApplicationUser?>)messagesQuery.Where(m => m.IsRead == (isRead.Value ? 1 : 0));
            }

            int totalCount = await messagesQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var messages = await messagesQuery
                               .OrderBy(m => m.CreationTime) // Oldest to newest
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

            var response = new PagedResponse<Message> 
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = messages,
                IsActive = (int?)YesNo.Yes,
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = _httpContextAccessor.HttpContext != null
                            ? CurrentUserName(_httpContextAccessor.HttpContext)
                            : "Unknown User"
            };

            return Ok(response);
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var currentUserName = CurrentUserName(_httpContextAccessor.HttpContext);
            message.SenderId = currentUserName;

            message.CreationTime = DateTime.UtcNow.ToString();

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // PUT: api/Messages/5 (Example: Marking a Message as Read)
        [HttpPut("{id}")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var messageToUpdate = await _context.Messages.FindAsync(id);
            if (messageToUpdate == null)
            {
                return NotFound("Message not found.");
            }

            var currentUserName = CurrentUserName(_httpContextAccessor.HttpContext);
            if (messageToUpdate.RecipientId != currentUserName)
            {
                return Unauthorized("You are not authorized to modify this message.");
            }

            messageToUpdate.IsRead = 1;

            _context.Entry(messageToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Messages/5 
        public async Task<IActionResult> DeleteMessage(int id)
        {

            var messageToDelete = await _context.Messages.FindAsync(id);
            if (messageToDelete == null)
            {
                return NotFound("Message not found.");
            }

            var currentUserName = CurrentUserName(_httpContextAccessor.HttpContext);
            if (messageToDelete.SenderId != currentUserName && messageToDelete.RecipientId != currentUserName)
            {
                return Unauthorized("You are not authorized to delete this message.");
            }

            if (messageToDelete.SenderId == currentUserName)
                messageToDelete.IsDeletedForSender = 1;

            if (messageToDelete.RecipientId == currentUserName)
                messageToDelete.IsDeletedForRecipient = 1;

            if (messageToDelete.IsDeletedForSender == 1 && messageToDelete.IsDeletedForRecipient == 1)
            {
                _context.Messages.Remove(messageToDelete); // Perform hard delete
            }
            else
            {
                _context.Entry(messageToDelete).State = EntityState.Modified; // Soft-update flags
            }

            await _context.SaveChangesAsync();


            return NoContent();
        }
    }
}