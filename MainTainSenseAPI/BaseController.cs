using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MainTainSenseAPI.Controllers // Adjust if your controllers are in a different namespace
{
    public abstract class BaseController : ControllerBase
    {
        protected string CurrentUserName => GetCurrentUserName(HttpContext);

        private string GetCurrentUserName(HttpContext context)
        {
            return context.User?.Identity?.Name ?? "jamieDevelopment"; // Placeholder for now
        }

        protected Serilog.ILogger _logger;
        private readonly IConfiguration _configuration;

        protected BaseController(Serilog.ILogger logger, IConfiguration configuration) // Add parameter here
        {
            _logger = Log.ForContext<BaseController>();
            _configuration = configuration; // Assign to the field
        }

        protected string GetBaseUrl()
        {
            return _configuration["BaseUrl"] ?? "http://default-base-url";
        }
    }
}
