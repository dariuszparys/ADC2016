using Microsoft.AspNetCore.Builder;

namespace DryRun
{
    public static class RequestAdExtensions
    {
        public static IApplicationBuilder UseRequestAds(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestAdMiddleware>();
        }
    }
}