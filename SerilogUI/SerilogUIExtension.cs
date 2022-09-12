﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Reflection;

namespace SerilogUI
{
    public static class SerilogUIExtension
    {
        public static IServiceCollection AddSerilogUI(this IServiceCollection services, Action<SerilogUIConfig>? configFactory)
        {
            var config = new SerilogUIConfig();
            if (configFactory != null) {
                configFactory.Invoke(config);
            }

            services.AddScoped<SerilogUIRepository>();

            Log.Logger = new LoggerConfiguration()
              .WriteTo.File(new JsonFormatter(), config.LogPath + "/.log", Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day)
              .CreateLogger();
            services.AddTransient(serviceProvider => Log.Logger);

            return services;
        }

        public static IEndpointRouteBuilder UseSerilogUIDashboard(this IEndpointRouteBuilder app, Action<SerilogUIConfig>? configFactory)
        {
            var config = new SerilogUIConfig();
            if (configFactory != null) {
                configFactory.Invoke(config);
            }

            app.MapControllerRoute("SerilogUI", "{controller=SerilogUI}/{action=Index}");

            return app;
        }
    }

}
