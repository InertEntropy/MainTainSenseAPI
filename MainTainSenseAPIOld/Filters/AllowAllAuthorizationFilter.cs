using Microsoft.AspNetCore.Mvc.Filters;

namespace MainTainSenseAPI.Filters // Adjust namespace if you place it elsewhere
{
    public class AllowAllAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Do nothing - always allow access
        }
    }
}
