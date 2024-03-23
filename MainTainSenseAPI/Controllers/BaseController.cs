using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace MainTainSenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController: ControllerBase
    {
        protected readonly ILogger<BaseController> logger;
        protected readonly IConfiguration configuration;
        public Func<HttpContext, string> CurrentUserName { get; }


        // Single Constructor
        protected BaseController(ILogger<BaseController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base() // Call the base constructor of ControllerBase
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Initialize CurrentUserName
            CurrentUserName = (context) => {
                var username = context?.User?.Identity?.Name; // Null-conditional access
                return username ?? "Unknown User";
            };
        }

        protected string GetBaseUrl()
        {
            var baseUrl = configuration["BaseUrl"];
            return !string.IsNullOrEmpty(baseUrl) ? baseUrl : "http://default-base-url";
        }

        protected virtual IActionResult HandleException(Exception ex)
        {
            if (ex is DbException dbException)
            {
                return HandleDatabaseException(dbException, 0, null); // Provide defaults for entityId and role
            }

            if (ex.InnerException is SqliteException sqliteEx)
            {
                var problemDetails = new ProblemDetails();

                switch (sqliteEx.ErrorCode)
                {
                    case 19: // Constraint violation
                        problemDetails.Status = 400; // BadRequest
                        problemDetails.Title = "Cannot create";
                        problemDetails.Detail = "Cannot create already exists";
                        break;

                    case 1: // Database locked
                            // ...
                        problemDetails.Status = 503; // ServiceUnavailable
                        problemDetails.Title = "Database temporarily unavailable";
                        problemDetails.Detail = "Database temporarily busy, please try again";
                        break;

                    case 6:  //  Cannot open
                        problemDetails.Status = 500; // ServiceUnavailable
                        problemDetails.Title = "Failed to open database";
                        problemDetails.Detail = "Database error, contact administrator";
                        break;

                    case 11:  // Database is corrupt
                        problemDetails.Status = 500; // ServiceUnavailable
                        problemDetails.Title = "Database corruption detected. Application terminating.";
                        problemDetails.Detail = "Critical database failure, contact support.";
                        break;

                    default:
                        problemDetails.Status = 500; // InternalServerError
                        problemDetails.Title = "Database error";
                        problemDetails.Detail = "An unhandled database error occurred.";
                        break;
                }
                logger.LogError(ex, "Database error: {details}", problemDetails.Detail);
                return new ObjectResult(problemDetails);
            }
            else if (ex is not null)
            {
                // Catch-all for more generic database errors
                logger.LogError(ex, "A database error occurred: {errorMessage}", ex.Message);
                return StatusCode(500, "A database error occurred. Please try again or contact support.");
            }
            else
            {
                // Existing fallback 
                logger.LogError(ex, "Unhandled Database Exception");
                return StatusCode(500, "Error processing request");
            }
        }
        protected virtual IActionResult HandleDatabaseException(DbException ex, int entityId, ApplicationUser? role = null)
        {
            // Default database exception handling 
            logger.LogError(ex, "Database exception occurred.");
            return StatusCode(500, "A database error occurred.");
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
            logger.LogWarning("Validation errors occurred: {errors}", problemDetails.Extensions["errors"]);
            return BadRequest(problemDetails);
        }
        
        protected virtual IActionResult HandleConcurrencyError(int entityId)
        {
            var problemDetails = new ProblemDetails
            {
                Status = 409, // Conflict
                Title = "Concurrency Conflict",
                Detail = $"The record with ID '{entityId}' has been modified since you retrieved it. Please refresh and try again."
            };

            return new ObjectResult(problemDetails);
        }
    }
}