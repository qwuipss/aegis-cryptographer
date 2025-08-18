using System.Collections.Immutable;
using Aegis.Cli.Exceptions.Algorithms;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Services.Logging;
using Aegis.Core.Algorithms;
using Aegis.Core.Services;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Algorithms;

internal sealed class AlgorithmResolver(
    ILogger<AlgorithmResolver> logger,
    ILogger<InlineLogger> inlineLogger,
    ICryptoService cryptoService,
    IConsoleReader consoleReader
) : IAlgorithmResolver
{
    private readonly ILogger<AlgorithmResolver> _logger = logger;
    private readonly ILogger<InlineLogger> _inlineLogger = inlineLogger;
    private readonly ICryptoService _cryptoService = cryptoService;
    private readonly IConsoleReader _consoleReader = consoleReader;

    public IAlgorithm Resolve(string token)
    {
        var algorithm = token switch
        {
            AlgorithmTokens.Rune => CreateAesGcmAlgorithm(),
            _ => throw new AlgorithmNotResolvedException(token),
        };

        _logger.LogDebug("Resolved algorithm '{algorithm}'", algorithm.GetType().Name);

        return algorithm;
    }

    private RuneAlgorithm CreateAesGcmAlgorithm()
    {
        _inlineLogger.LogInformation("Enter secret: ");
        var secret = _consoleReader.ReadSecret();
        return new RuneAlgorithm([..secret], _cryptoService);
    }
}