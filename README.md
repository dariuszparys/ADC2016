# ASP.NET Core Demo for ADC 2016 - Step by step Instructions

## Building a first ASPNETCORE application

Creating the skeleton to work on

```
mkdir demo
cd demo
dotnet new
dotnet restore
dotnet run
```

Open the project with Visual Studio Code and enhance `project.json` with the corresponding reference

```
"Microsoft.AspNetCore.Server.Kestrel": "1.0.1"
```

Modifying `Program.cs`

```
using System;
using Microsoft.AspNetCore.Hosting;

namespace DryRun
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
```

Adding `Startup.cs` to the project with this content

```
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DryRun
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello from the Core platform!");
            });
        }
    }
}
```

Start the app through the Visual Studio Code Debugger.

## Add Logging

Goto `Startup.cs` and change the function signature to the following

```
public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
```

Add logging to the application

```
loggerFactory.AddConsole();
var logger = loggerFactory.CreateLogger("DEMO");
// inside request
...
logger.LogInformation("WebRequest is coming...");
```

To have output shown on the console you need the Console Logging extension, add it to `project.json`

```
"Microsoft.Extensions.Logging.Console": "1.0.0"
```

## Demonstrating Middleware workflow

Just add a inline middleware method with log functions to demonstrate the pipeline approach

```
app.Use( async ( context, next ) => 
{
    logger.LogInformation( "My custom middleware" );
    await next.Invoke();
    logger.LogInformation( "Pipeline is returning" );
}
```

## Show alternate configurations using `Map`

Add the following function to `Startup.cs`

```
public void ConfigureAlternate(IApplicationBuilder app)
{
    app.Run( async context => 
    {
        await context.Response.WriteAsync("Welcome to the Advanced Developers Conference 2016!");
    });
}
```

Register the route in the `Configure` function

```
app.Map( "/message", ConfigureAlternate );
```

## Writing an Middleware

Implement a simple Middleware example, in this case a SPAM Middleware redirecting to a website 
when a specific keyword is found in the end of the querystring 

```
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
```

Add the extension for `IApplicationBuilder`

```
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
```

And use the Middleware inside the startup configurations

```
app.UseRequestAds();
```

To trigger the new middleware just enter http://localhost:5000/?spam

## Creating a service for dependency injection

Add a new interface and an implementation. This sample is an homage to 
old COM times.

IUnknown interface definition

```
namespace DryRun
{
    public interface IUnknownService
    {
        void AddRef();
        string QueryInterface(string key);
        void Release();
    }
}
```

And the corresponding implementation

```
using System;

namespace DryRun
{
    public class UnknownService : IUnknownService
    {
        private int count = 0;

        public void AddRef()
        {
            count++;
        }

        public string QueryInterface(string key)
        {
            return $"Do you really? WTF! Anyway, the current count is {count}";
        }

        public void Release()
        {
            count--;
        }
    }
}
```

Add the service to the dependency injection framework in the `Startup` class


```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IUnknownService, UnknownService>();
}
```
## Adding MVC

Let's add ASP.NET MVC capabilities. First reference in `project.json` the
relevant Nuget package

```
"Microsoft.AspNetCore.Mvc": "1.0.1"
```

Then initialize ASP.NET MVC in `Startup`

```
services.AddMvc();

app.UseMvcWithDefaultRoute();
```

Adding a controller and a model to return data. First the model

```
namespace DryRun
{
    public class Results
    {
        public string Title { get; set; }
        public string Value { get; set; }

    }
}
```

and than the controller using the model. 

```
using Microsoft.AspNetCore.Mvc;

namespace DryRun
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new Results();
            model.Title = "Welcome";
            model.Value = "n/a";
            return Json(model);
        }
    }
}
```

Finally let's add the `UnknownService` to the controller via DI.

```
private readonly IUnknownService service;
public HomeController(IUnknownService service)
{
    this.service = service;
}
```

and fill the model with the result of this service

```
service.AddRef();
var model = new Results();
model.Title = "Welcome";
model.Value = $"Reference counts so far {service.QueryInterface("IID_IDispatch")}";
```

## That's it

That is currently the most basic walkthrough, there are a lot of basic areas missing
like for instance the whole security topic