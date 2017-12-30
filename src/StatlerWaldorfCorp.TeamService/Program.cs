using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace StatlerWaldorfCorp.TeamService
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseConfiguration(config)
                .Build();

            host.Run();
        }
    }
}
