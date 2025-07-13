using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Extensions;

internal static class LoggerExtensions
{
    public static ILogger<TContext> ToSecret<TContext>(this ILogger<TContext> logger)
    {
        return logger.For
    }
}