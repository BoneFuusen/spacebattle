using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
class Program()
{
    static void Main(string[] args)
    {
        IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args)
        .UseKestrel(options =>
        {
            options.ListenAnyIP(8080);
        })
        .UseStartup<Startup>();

        IWebHost app = builder.Build();
        app.Run();
    }
}