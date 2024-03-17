using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Authorize] // Require authentication for actions in this controller
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, ILogger<UsersController> logger,
                IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(logger, configuration, httpContextAccessor)  // Pass dependencies
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive == 1) // Filter active users
                .Include(u => u.JobTitle)
                .Include(u => u.Department)
                .Select(u => new {
                    User = u,
                    Roles = _userManager.GetRolesAsync(u).Result
                })
                .ToListAsync();

            var viewModels = users.Select(u => new UserViewModel
            {
                Id = u.User.Id,
                UserName = u.User.UserName,
                FirstName = u.User.FirstName ?? "",
                LastName = u.User.LastName ?? "",
                JobTitle = u.User.JobTitle?.Name ?? "None Assigned",
                IsActive = u.User.IsActive ?? 0,
                Department = u.User.Department?.Name ?? "None Assigned",
                Email = u.User.Email ?? "",
                LastUpdated = u.User.LastUpdated,
                UpdatedBy = u.User.UpdatedBy,
                Roles = u.Roles
            });

            return Ok(viewModels);
        }
        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsActive != 1) { return NotFound(); } // Check IsActive

            var viewModel = new UserViewModel { /* Map from 'user' */ };
            return Ok(viewModel);
        }
    }
}
