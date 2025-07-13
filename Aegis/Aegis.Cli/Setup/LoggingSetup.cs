using Aegis.Cli.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Aegis.Cli.Setup;

internal static class LoggingSetup
{
    public static IServiceCollection SetupUtilityLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .Enrich.With<SourceContextShortenedEnricher>()
                     .WriteTo.Console(
                         outputTemplate: "{Timestamp:HH:mm:ss} {Message:lj}{NewLine}",
                         standardErrorFromLevel: LogEventLevel.Error,
#if DEBUG
                         restrictedToMinimumLevel: LogEventLevel.Debug
#else
                         restrictedToMinimumLevel: LogEventLevel.Information
#endif
                     )
                     .WriteTo.File(
                         outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] [{SourceContextShortened}] {Message:lj}{NewLine}{Exception}",
                         path: LogsHelper.GetLogFilePath(),
                         rollingInterval: RollingInterval.Infinite,
                         fileSizeLimitBytes: 8 * 1024 * 1024,
                         buffered: false,
                         rollOnFileSizeLimit: true,
                         flushToDiskInterval: TimeSpan.FromSeconds(3)
                     )
                     .WriteTo.Logger(cfg => cfg.Filter.ByExcluding())
                     .CreateLogger();

        services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            }
        );

        return services;
    }

    private class SourceContextShortenedEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var sourceContext = logEvent.Properties.TryGetValue("SourceContext", out var contextValue)
                ? contextValue.ToString().AsSpan().Trim('"')
                : null;

            if (sourceContext.IsEmpty)
            {
                return;
            }

            var dotLastIndex = sourceContext.LastIndexOf('.');
            var sourceContextShortened = sourceContext[(dotLastIndex + 1)..].ToString();
            var logEventProperty = propertyFactory.CreateProperty("SourceContextShortened", sourceContextShortened);
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}