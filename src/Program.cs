using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddFile("/logs/rt_internal_control_service-{Date}.log");
                })
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
