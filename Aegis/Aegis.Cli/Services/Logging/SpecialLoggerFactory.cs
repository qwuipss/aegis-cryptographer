using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Logging;

internal sealed class SpecialLoggerFactory(ILoggerFactory loggerFactory) : ISpecialLoggerFactory
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public ILogger CreateSecretLogger()
    {
        return _loggerFactory.CreateLogger<SecretLogger>();
    }

    public ILogger CreateInlineLogger()
    {
        return _loggerFactory.CreateLogger<InlineLogger>();
    }
}