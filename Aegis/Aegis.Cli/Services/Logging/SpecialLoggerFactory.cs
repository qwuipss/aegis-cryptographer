using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Logging;

internal sealed class SpecialLoggerFactory(ILogger<SecretLoggerContext> secretLogger) : ISpecialLoggerFactory
{
    private readonly ILogger<SecretLoggerContext> _secretLogger = secretLogger;
    
    public ILogger<SecretLoggerContext> CreateSecretLogger()
    {
        return _secretLogger;
    }
}