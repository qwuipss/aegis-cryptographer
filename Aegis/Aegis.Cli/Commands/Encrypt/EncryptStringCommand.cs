using Aegis.Cli.Extensions;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Encrypt;

internal sealed class EncryptStringCommand(ILogger<EncryptStringCommand> logger, ILogger<SecretLogger> secretLogger, IAlgorithmResolver algorithmResolver)
    : BaseCommand(logger)
{
    private readonly ILogger<EncryptStringCommand> _logger = logger;
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;
    private readonly IAlgorithmResolver _algorithmResolver = algorithmResolver;

    public override void Validate()
    {
        Parameters.ShouldContainSingleParameter();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override Task ExecuteAsync()
    {
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value ?? AlgorithmTokens.Aes.Medium;
        var algorithm = _algorithmResolver.Resolve(algorithmToken);
        _secretLogger.LogInformation("Encrypted value: {value}", algorithm);
        return Task.CompletedTask;
    }
}