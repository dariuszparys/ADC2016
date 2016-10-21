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

