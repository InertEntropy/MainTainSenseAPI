using MainTainSenseAPI.Data;
using MainTainSenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MainTainSenseAPI.Filters
{
    public class IsActiveFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext? _dbContext;

        public IsActiveFilterAttribute()
        {
        }

        public IsActiveFilterAttribute(ApplicationDbContext? dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_dbContext != null)
            {
                foreach (var entry in _dbContext.ChangeTracker.Entries())
                {
                    if (entry.Entity is BaseModel baseModel && baseModel.IsActive != YesNo.No)
                    {
                        context.Result = new BadRequestObjectResult("Entity is not active");
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}

