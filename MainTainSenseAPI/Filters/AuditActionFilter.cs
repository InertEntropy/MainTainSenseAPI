using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MainTainSenseAPI.Filters
{
    public class AuditActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments.Values.FirstOrDefault(v => v is BaseModel);
            if (model is BaseModel baseModel)
            {
                baseModel.LastUpdated = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                baseModel.UpdatedBy = context.HttpContext?.User?.Identity?.Name ?? "System";
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Empty for now
        }
    }

}
