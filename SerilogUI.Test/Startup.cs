using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SerilogUI.Test
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private static string EnvironmentName;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            EnvironmentName = env.EnvironmentName;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var logPath = Configuration.GetSection("EnvVars").GetValue<string>("Logging:Path") ?? "Logs";
            services.AddSerilogUI(conf => conf.SetLogDirectory(logPath));
            services.AddMvc();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogUIDashboard(conf => conf.SetPath("/SerilogUI"));
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Pesta! ;)");
                });
                endpoints.MapControllers();
            });
        }
    }
}