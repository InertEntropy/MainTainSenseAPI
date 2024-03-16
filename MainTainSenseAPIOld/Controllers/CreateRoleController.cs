using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MainTainSenseAPI.Models; 

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")] // Adjust route prefix if needed
    [ApiController]
    public class RolesController : BaseController // Inherits from BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly CustomRoleStore _roleStore; // Assuming your CustomRoleStore

        public RolesController(RoleManager<ApplicationRole> roleManager, CustomRoleStore roleStore, MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
            : base(context, logger, configuration)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Handle model validation errors
            }

            var role = new ApplicationRole { Name = model.RoleName }; // Map from ViewModel

            IdentityResult identityResult = await _roleStore.CreateAsync(role, CancellationToken.None); // Using your CustomRoleStore

            if (identityResult.Succeeded)
            {
                return Ok("Role created successfully"); // Or a more descriptive response
            }
            else
            {
                return HandleIdentityError(identityResult);  // Handle Identity errors
            }
        }
    }
}

