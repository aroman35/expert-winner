using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;

namespace WebApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                    configuration
                        .Enrich.WithExceptionDetails()
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("OperationSystem", Environment.OSVersion.VersionString)
                        .Enrich.WithProperty("User", Environment.UserName)
                        .Enrich.WithProperty("MachineName", Environment.MachineName)
                        .ReadFrom.Configuration(context.Configuration)
                )
                .ConfigureWebHost(builder => builder
                    .ConfigureServices(services => services
                        .AddControllers().Services
                        .AddSingleton(_ => new ServiceIdProvider(Guid.NewGuid()))
                        .AddSingleton<HostsManager>()
                        .AddSingleton<SomeSampleService>()
                        .AddHostedService(provider => provider.GetRequiredService<SomeSampleService>())
                        .AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication", Version = "v1" }); })))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}