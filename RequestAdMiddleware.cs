using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DryRun
{
    public class RequestAdMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestAdMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<RequestAdMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.QueryString.HasValue)
            {
                if (context.Request.QueryString.Value.EndsWith("spam"))
                {
                    context.Response.Clear();
                    context.Response.Redirect("https://pwnd.io");
                }
            }

            await this.next.Invoke(context);
        }
    }
}