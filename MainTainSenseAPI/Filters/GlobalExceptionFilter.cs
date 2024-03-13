using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MainTainSenseAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        
        private readonly Serilog.ILogger _logger;

        public GlobalExceptionFilter(Serilog.ILogger logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.Information(context.Exception, "Unhandled exception occurred");
            // You can customize the error response here
        }
    }
}
