using Hangfire.Dashboard;

namespace BMedia.API.Extensions;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        // Allow Hangfire dashboard only in development or for Administrator role
        return httpContext.User.IsInRole("Administrator") ||
               httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
    }
}
