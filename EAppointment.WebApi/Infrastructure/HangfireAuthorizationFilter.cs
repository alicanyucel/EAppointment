using Hangfire.Dashboard;

namespace EAppointment.WebApi.Infrastructure;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In production, implement proper authentication
        // For now, allow all in development
        var httpContext = context.GetHttpContext();
        return httpContext.Request.Host.Host == "localhost" || 
               httpContext.User.Identity?.IsAuthenticated == true;
    }
}
