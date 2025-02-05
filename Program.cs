using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Channel;

namespace SimpleWebJob
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageBlobs();
                    b.AddAzureStorageQueues();
                })
                .ConfigureLogging((context, b) =>
                {
                    b.AddConsole();
                    //b.AddApplicationInsightsWebJobs(o => o.ConnectionString = Environment.GetEnvironmentVariable("InstrumentationKey=fa8c28b8-ebfd-42e5-88e3-c48f1fdcebad;IngestionEndpoint=https://canadacentral-1.in.applicationinsights.azure.com/;LiveEndpoint=https://canadacentral.livediagnostics.monitor.azure.com/;ApplicationId=405de63e-1df7-4482-a6c9-03829bfa0628"));
                    b.AddApplicationInsightsWebJobs(o => o.ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING"));
                    b.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();
                })
                .Build();

            using (host)
            {                
               var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("WebJob started");

                // Simulate some work
                await Task.Delay(5000);

                logger.LogInformation("WebJob completed successfully");

                // Stop the host after the job completes
                await host.StopAsync();
            }
        }
    }

    public class MyTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = "MyWebJob6";
        }
    }
}