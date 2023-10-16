using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace Common.Loggin
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> configuration => (context, logConfiguration) =>
        {
            var seqServerUrl = context.Configuration["SeqServerUrl"];
            var logStashUrl = context.Configuration["LogStashUrl"];

            logConfiguration.MinimumLevel.Verbose()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(), "log.ndjson",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logStashUrl) ? "http://logstash:8080" : logStashUrl, null)
                .ReadFrom.Configuration(context.Configuration);

        };
    }
}