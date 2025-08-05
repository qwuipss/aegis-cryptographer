using Aegis.Cli.Exceptions.Algorithms;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Services.Logging;
using Aegis.Core.Algorithms;
using Aegis.Core.Algorithms.Aes0;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Algorithms;

internal sealed class AlgorithmResolver(ILogger<AlgorithmResolver> logger, ILogger<InlineLogger> inlineLogger, IConsoleReader consoleReader) : IAlgorithmResolver
{
    private readonly ILogger<AlgorithmResolver> _logger = logger;
    private readonly ILogger<InlineLogger> _inlineLogger = inlineLogger;
    private readonly IConsoleReader _consoleReader = consoleReader;

    public IAlgorithm Resolve(string token)
    {
        var algorithm = token switch
        {
            AlgorithmTokens.Aes.Medium => CreateAes0MAlgorithm(),
            _ => throw new AlgorithmNotResolvedException(token),
        };

        _logger.LogDebug("Resolved algorithm '{algorithm}'", algorithm.GetType().Name);

        return algorithm;
    }

    private Aes0MediumAlgorithm CreateAes0MAlgorithm()
    {
        _inlineLogger.LogInformation("Enter secret: ");
        var secret = _consoleReader.ReadSecret();
        return new Aes0MediumAlgorithm();
    }
}