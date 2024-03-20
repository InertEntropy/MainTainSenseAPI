using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using MainTainSenseAPI.Models.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ILogger<UsersController> logger, IConfiguration configuration,
                               ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                               IHttpContextAccessor httpContextAccessor)
            : base(logger, configuration, httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                            .Select(u => new
                            {
                                u.Id,
                                u.UserName,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                JobTitle = u.JobTitle != null ? u.JobTitle.Name : "Not Assigned",
                                Department = u.Department != null ? u.Department.Name : "Not Assigned",
                                Roles = _userManager.GetRolesAsync(u).Result // Get roles of a user
                            })
                            .ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitleId = user.JobTitleId,
                DepartmentId = user.DepartmentId,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return Ok(userViewModel);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        JobTitleId = model.JobTitleId ?? 0,
                        DepartmentId = model.DepartmentId ?? 0,
                    };
                    if (model.RolesId != null)
                    {
                        var selectedRoleNames = model.AvailableRoles?
                             .Where(r => r.Value == model.RolesId && r.Text != null) // Ensure Text isn't null
                             .Select(r => r.Text)
                             .Where(text => text != null) // Filter out null values
                             .ToList() ?? new List<string?>();

                        // Ensure that the list contains only non-null strings
                        var nonNullableRoles = selectedRoleNames.Where(role => role != null).Select(role => role!).ToList();

                        await _userManager.AddToRolesAsync(user, nonNullableRoles);
                    }
                    else
                    {
                        // Handle the case where model.UserName is null or empty 
                        return BadRequest("Username is required"); // Example handling
                    }
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        if (model.Roles != null)
                        {
                            await _userManager.AddToRolesAsync(user, model.Roles);
                        }

                        logger.LogInformation("User created: {UserName}", user.UserName);
                        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
                    }
                    else
                    {
                        return HandleIdentityError(result);
                    }
                }
            }
            return Ok("User created successfully");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update basic properties
            user.UserName = model.UserName;
            user.Email = model.Email;
            // ... (Other properties)

            // Update Roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, model.Roles ?? new List<string>());

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                logger.LogInformation("User updated: {UserName}", user.UserName);
                return NoContent();
            }
            else
            {
                return HandleIdentityError(updateResult);
            }
        }
    }
}

