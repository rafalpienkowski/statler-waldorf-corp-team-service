using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using StatlerWaldorfCorp.TeamService.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StatlerWaldorfCorp.TeamService.LocationClient;
using Microsoft.Extensions.Configuration.Json;

namespace StatlerWaldorfCorp.TeamService
{
    public class Startup
    {
        public static string[] Args { get; set; } = new string[] { };
        public IConfiguration Configuration { get; }
        private ILogger logger;
        private ILoggerFactory loggerFactory;
        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);

            Configuration = builder.Build();

            this.loggerFactory = loggerFactory;
            this.loggerFactory.AddConsole(LogLevel.Information);
            this.loggerFactory.AddDebug();

            this.logger = this.loggerFactory.CreateLogger("Startup");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<ITeamRepository, MemoryTeamRepository>();
            var locationUrl = Configuration.GetSection("location:url").Value;
            logger.LogInformation($"Using {locationUrl} for location service URL.");
            services.AddSingleton<ILocationClient>(new HttpLocationClient(locationUrl));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}