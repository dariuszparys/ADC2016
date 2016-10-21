using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DryRun
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUnknownService, UnknownService>();
            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            var logger = loggerFactory.CreateLogger("DEMO");

            app.Map("/message", ConfigureAlternate);

            app.Use(async (context, next) =>
            {
                logger.LogInformation("My custom middleware");
                await next.Invoke();
                logger.LogInformation("Pipeline is returning");
            });

            app.UseRequestAds();

            app.UseMvcWithDefaultRoute();

            app.Run(context =>
            {
                logger.LogInformation("WebRequest coming in...");
                return context.Response.WriteAsync("Hello from the Core platform!");
            });
        }

        public void ConfigureAlternate(IApplicationBuilder app)
        {
            app.Run(async context =>
           {
               await context.Response.WriteAsync("Welcome to Advanced Developers Conference 2016!");
           });
        }
    }
}