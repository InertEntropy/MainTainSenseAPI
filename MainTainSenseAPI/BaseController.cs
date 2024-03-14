using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MainTainSenseAPI.Controllers // Adjust if your controllers are in a different namespace
{
    public abstract class BaseController(MainTainSenseDataContext context, Serilog.ILogger logger, IConfiguration configuration) : ControllerBase
    {
        protected string CurrentUserName => GetCurrentUserName(HttpContext);
        protected Serilog.ILogger _logger;
        protected readonly MainTainSenseDataContext _context;

        private string GetCurrentUserName(HttpContext context)
        {
            return context.User?.Identity?.Name ?? "jamieDevelopment"; // Placeholder for now
        }
        
        protected string GetBaseUrl()
        {
            return configuration["BaseUrl"] ?? "http://default-base-url";
        }

        protected IActionResult HandleDatabaseException(DbUpdateException ex)
        {
            
            if (ex.InnerException is SqliteException sqliteEx)
            {
                switch (sqliteEx.ErrorCode)
                {
                    case 19:  // Example: SQLite Constraint Violation
                        return BadRequest("Cannot update due to related data");

                    case 1:  // Database locked
                        return StatusCode(503, "Database temporarily busy, please try again");

                    case 6:  //  Cannot open
                        _logger.Error(sqliteEx, "Failed to open database");
                        return StatusCode(500, "Database error, contact administrator");

                    case 11:  // Database is corrupt
                        _logger.Fatal(sqliteEx, "Database corruption detected. Application terminating."); // Use LogFatal for critical errors
                        return StatusCode(500, "Critical database failure, contact support.");

                    default:
                        _logger.Error(ex, "SQLite Error");
                        return StatusCode(500);
                }
            }
            else
            {
                _logger.Error(ex, "Unhandled Database Exception");
                return StatusCode(500, "Error processing request");
            }
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
                // Shouldn't get here if ModelState is invalid
                throw new Exception("Unexpected error - ModelState is valid in HandleValidationErrors");
            }

            problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();

            foreach (var entry in modelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    var errorMessages = entry.Value.Errors.Select(e => e.ErrorMessage).ToArray();

                    // Ensure 'errors' is an array dictionary
                    if (!problemDetails.Extensions.ContainsKey("errors"))
                    {
                        problemDetails.Extensions["errors"] = new Dictionary<string, string[]>();
                    }

                    var errorsDict = problemDetails.Extensions["errors"] as Dictionary<string, string[]>;
                    errorsDict.Add(entry.Key, errorMessages);
                }
            }

            return BadRequest(problemDetails);
        }
    }
}

