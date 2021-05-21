using DotNetCore.CAP.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Cap
{
    public class CapAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public Task<bool> AuthorizeAsync(DashboardContext context)
        {
            var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();
            //return Task.FromResult(httpContextAccessor.HttpContext.User.Identity.IsAuthenticated);
            return Task.FromResult(true);
        }
    }
}
