using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace WebApplication
{
    public class HostFactory
    {
        public static IHost CreateHost(Stream appSettings, string environment, Guid serverId)
        {
            var builder = Host.CreateDefaultBuilder()
                .UseEnvironment(environment)
                .ConfigureAppConfiguration(builder => builder.AddJsonStream(appSettings))
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
                .ConfigureServices(services =>
                    services
                        .AddSingleton(_ => new ServiceIdProvider(serverId))
                        .AddSingleton<SomeSampleService>()
                        .AddHostedService(provider => provider.GetRequiredService<SomeSampleService>()));

            return builder.Build();
        }
    }
}