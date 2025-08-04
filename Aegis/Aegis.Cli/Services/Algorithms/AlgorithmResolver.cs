using Aegis.Core.Algorithms;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Algorithms;

internal sealed class AlgorithmResolver(ILogger<AlgorithmResolver> logger) : IAlgorithmResolver
{
    private readonly ILogger<AlgorithmResolver> _logger = logger;
    
    public IAlgorithm Resolve(string algorithmName)
    {
        var algorithm = algorithmName switch
        {
            AlgorithmTokens.Aes.Medium => new Aes0MAlgorithm(),
        };
        
        _logger.LogInformation("Resolved algorithm '{algorithm}'", algorithm.GetType().Name);
        
        return algorithm;
    }

    private static Aes0MAlgorithm CreateAes0MAlgorithm()
    {
        // var secret = 
        return new Aes0MAlgorithm();
    }
}