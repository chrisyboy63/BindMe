using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using BindMe.Process;

namespace BindMe.ConsoleApp
{
    class Program
    {
        const string AppName = "BindMe";
        static Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created.");
            DefaultProcess dp = new DefaultProcess(logger);
            dp.init();

            return host.RunAsync();
        }
    }
}
