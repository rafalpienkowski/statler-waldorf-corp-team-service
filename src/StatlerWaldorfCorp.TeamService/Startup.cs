using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using StatlerWaldorfCorp.TeamService.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace StatlerWaldorfCorp.TeamService
{
    internal class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<ITeamRepository, MemoryTeamRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello Word from container!");
            });
        }
    }
}