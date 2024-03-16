using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MainTainSenseAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
                protected readonly MainTainSenseDataContext _context;
        protected Serilog.ILogger? _logger;
        protected readonly IConfiguration _configuration;

        public Func<HttpContext, string> CurrrentUserName { get; }

        protected BaseController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            CurrrentUserName = GetCurrentUserName;
        }

        protected string GetCurrentUserName(HttpContext context)
        {
            return context.User?.Identity?.Name ?? "jamieDevelopment";
        }

        protected string GetBaseUrl()
        {
            var baseUrl = _configuration["BaseUrl"];
            return !string.IsNullOrEmpty(baseUrl) ? baseUrl : "http://default-base-url";
        }

        protected IActionResult HandleDatabaseException(DbUpdateException ex, ApplicationRole? role = null)
        {
            role ??= new ApplicationRole();

            if (ex.InnerException is SqliteException sqliteEx)
            {
                switch (sqliteEx.ErrorCode)
                {
                    case 19: // Constraint violation
                        if (sqliteEx.Message.Contains("foreign key constraint failed"))
                        {
                            _logger?.Error(sqliteEx, "Failed to create role {RoleName} due to existing references.", role?.Name ?? "(Unknown Role)");
                            return HandleIdentityError(IdentityResult.Failed(new IdentityError { Description = "Cannot create role because it's assigned to existing users." })); // Use HandleIdentityError
                        }
                        else
                        {
                            _logger?.Error(sqliteEx, "Cannot update due to related data");
                            return BadRequest("Cannot update due to related data"); 
                        }

                    case 1:  // Database locked
                        _logger?.Error(sqliteEx, "Database temporarily busy, please try again");
                        return StatusCode(503, "Database temporarily busy, please try again");

                    case 6:  //  Cannot open
                        _logger?.Error(sqliteEx, "Failed to open database");
                        return StatusCode(500, "Database error, contact administrator");

                    case 11:  // Database is corrupt
                        _logger?.Fatal(sqliteEx, "Database corruption detected. Application terminating."); 
                        return StatusCode(500, "Critical database failure, contact support.");

                    default:
                        _logger?.Error(ex, "SQLite Error");
                        return StatusCode(500);
                }
            }
            else
            {
                _logger?.Error(ex, "Unhandled Database Exception");
                return StatusCode(500, "Error processing request");
            }
        }

        protected IActionResult HandleIdentityError(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description); // Integrate with ModelState
                }
                return BadRequest(ModelState); // Return a BadRequest with validation errors
            }

            return StatusCode(500, "An unhandled error occurred."); // Generic fallback
        }

        protected IActionResult HandleValidationErrors(ModelStateDictionary modelState)
        {
            var problemDetails = new ProblemDetails
            {
                Status = 400, // BadRequest
                Title = "One or more validation errors occurred.",
                Detail = "Please check the following fields:"

            };

            if (modelState.IsValid)
            {
                return BadRequest("Unexpected error - ModelState is valid in HandleValidationErrors");
            }

            problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();

            foreach (var entry in modelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    var errorMessages = entry.Value.Errors.Select(e => e.ErrorMessage).ToArray();

                    problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();

                    var errorsDict = problemDetails.Extensions["errors"] as Dictionary<string, string[]>;
                    errorsDict?.Add(entry.Key, errorMessages);
                }
            }
            _logger?.Warning("Validation errors occurred: {errors}", problemDetails.Extensions["errors"]);
            return BadRequest(problemDetails);
        }
        
    }
}

