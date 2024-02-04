using Hangfire.Dashboard;

namespace XPressPayments.Jobs.Service.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Check if the user is authenticated
            return context?.GetHttpContext()?.User?.Identity?.IsAuthenticated ?? false; //    .IsInRole("Admin");
        }
    }

}
