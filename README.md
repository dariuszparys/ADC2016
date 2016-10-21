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

Start the app through the Visual Studio Code Debugger

