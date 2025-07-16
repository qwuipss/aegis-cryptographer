using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Logging;

internal interface ISpecialLoggerFactory
{
    ILogger<SecretLoggerContext> CreateSecretLogger();
}