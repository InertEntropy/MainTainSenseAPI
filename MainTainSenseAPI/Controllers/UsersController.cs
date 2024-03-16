using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainTainSenseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    //[Authorize] // Secure users-only actions 
    public class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Temporary: Return username without database interaction
            return Ok(model.FirstName);
        }
    }
}
