using Aegis.Cli.Services.Logging;
using Aegis.Cli.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

namespace Aegis.Cli.Setup;

internal static class LoggingSetup
{
    private static readonly string InlineLoggerTypeFullName = typeof(InlineLogger).FullName!;

    public static IServiceCollection SetupUtilityLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Logger(logger =>
                                         logger
                                             .WriteTo.Console(
                                                 outputTemplate: "{Message:lj}{NewLine}",
                                                 standardErrorFromLevel: LogEventLevel.Error,
#if DEBUG
                                                 restrictedToMinimumLevel: LogEventLevel.Debug
#else
                                                 restrictedToMinimumLevel: LogEventLevel.Information
#endif
                                             )
                                             .Filter.ByExcluding(logEvent =>
                                                                     LogEventExtensions.TryGetStringValue(
                                                                         logEvent,
                                                                         "SourceContext",
                                                                         out var sourceContext
                                                                     )
                                                                     && sourceContext == InlineLoggerTypeFullName
                                             )
                     )
                     .WriteTo.Logger(logger =>
                                         logger
                                             .WriteTo.Console(
                                                 outputTemplate: "{Message:lj}",
                                                 restrictedToMinimumLevel: LogEventLevel.Information
                                             )
                                             .Filter.ByIncludingOnly(logEvent =>
                                                                         LogEventExtensions.TryGetStringValue(
                                                                             logEvent,
                                                                             "SourceContext",
                                                                             out var sourceContext
                                                                         )
                                                                         && sourceContext == InlineLoggerTypeFullName
                                             )
                     )
                     .WriteTo.Logger(logger =>
                                         logger
                                             .Enrich.With<SourceContextShortenedEnricher>()
                                             .Enrich.With<SecretLoggerPropertiesEraser>()
                                             .WriteTo.File(
                                                 outputTemplate:
                                                 "{Timestamp:yyyy-MM-dd.HH:mm:ss.fff} [{Level:u3}] [{SourceContextShortened}] {Message:lj}{NewLine}{Exception}",
                                                 path: LogsHelper.GetLogFilePath(),
                                                 rollingInterval: RollingInterval.Infinite,
                                                 fileSizeLimitBytes: 8 * 1024 * 1024,
                                                 buffered: false,
                                                 rollOnFileSizeLimit: true,
                                                 flushToDiskInterval: TimeSpan.FromSeconds(3)
                                             )
                     )
                     .CreateLogger();

        services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            }
        );
        
        return services;
    }

    private sealed class SourceContextShortenedEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            LogEventProperty logEventProperty;
            if (!LogEventExtensions.TryGetStringValue(logEvent, "SourceContext", out var sourceContext))
            {
                logEventProperty = propertyFactory.CreateProperty("SourceContextShortened", "<NoSourceContext>");
                logEvent.AddPropertyIfAbsent(logEventProperty);
                return;
            }

            var dotLastIndex = sourceContext!.LastIndexOf('.');
            var sourceContextShortened = sourceContext[(dotLastIndex + 1)..];
            logEventProperty = propertyFactory.CreateProperty("SourceContextShortened", sourceContextShortened);
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }

    private sealed class SecretLoggerPropertiesEraser : ILogEventEnricher
    {
        private static readonly string SecretLoggerTypeFullName = typeof(SecretLogger).FullName!;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!LogEventExtensions.TryGetStringValue(logEvent, "SourceContext", out var sourceContext)
                || sourceContext != SecretLoggerTypeFullName)
            {
                return;
            }

            var tokens = logEvent
                         .MessageTemplate.Tokens
                         .Where(t => t is PropertyToken)
                         .Cast<PropertyToken>()
                         .Select(pt => pt.PropertyName)
                         .ToHashSet();

            foreach (var token in tokens)
            {
                logEvent.RemovePropertyIfPresent(token);
            }
        }
    }

    private static class LogEventExtensions
    {
        public static bool TryGetStringValue(LogEvent logEvent, string propertyName, out string? value)
        {
            if (logEvent.Properties.TryGetValue(propertyName, out var property)
                && property is ScalarValue { Value: string { Length: not 0, } stringValue, })
            {
                value = stringValue;
                return true;
            }

            value = null;
            return false;
        }
    }
}