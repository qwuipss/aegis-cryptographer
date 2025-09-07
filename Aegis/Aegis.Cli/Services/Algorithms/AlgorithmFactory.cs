using System.Collections.Immutable;
using Aegis.Cli.Extensions;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Services.Logging;
using Aegis.Core.Algorithms;
using Aegis.Core.Services;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Algorithms;

internal sealed class AlgorithmFactory(
    ILogger<AlgorithmFactory> logger,
    ICryptoService cryptoService,
    IConsoleReader consoleReader
) : IAlgorithmFactory
{
    private readonly ILogger<AlgorithmFactory> _logger = logger;
    private readonly ICryptoService _cryptoService = cryptoService;
    private readonly IConsoleReader _consoleReader = consoleReader;

    public IAlgorithm Create(AlgorithmType algorithmType)
    {
        var algorithm = algorithmType switch
        {
            AlgorithmType.Rune => CreateRuneAlgorithm(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithmType)),
        };

        _logger.LogDebug("Resolved algorithm '{algorithm}'", algorithm.GetType().Name);

        return algorithm;
    }

    private RuneAlgorithm CreateRuneAlgorithm()
    {
        var secret = _consoleReader.ReadSecret("Enter secret");
        return new RuneAlgorithm([..secret.ToGlobalEncodingBytes(),], _cryptoService);
    }
}