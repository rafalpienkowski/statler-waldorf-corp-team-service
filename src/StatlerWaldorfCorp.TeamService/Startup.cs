using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using StatlerWaldorfCorp.TeamService.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StatlerWaldorfCorp.TeamService.LocationClient;
using StatlerWaldorfCorp.TeamService.Models;

namespace StatlerWaldorfCorp.TeamService
{
    public class Startup
    {
        public static string[] Args { get; set; } = new string[] { };
        public IConfiguration Configuration { get; }
        private ILogger _logger;
        private ILoggerFactory _loggerFactory;
        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);

            Configuration = builder.Build();

            _loggerFactory = loggerFactory;
            _loggerFactory.AddConsole(LogLevel.Information);
            _loggerFactory.AddDebug();

            _logger = _loggerFactory.CreateLogger("Startup");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc();

            services.Configure<MongoDbSettings>(Configuration.GetSection("mongodb"));

            services.AddSingleton<ITeamRepository, TeamRepository>();
            var locationUrl = Configuration.GetSection("location:url").Value;
            services.AddSingleton<ILocationClient>(new HttpLocationClient(locationUrl));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}