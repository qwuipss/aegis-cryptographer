using Aegis.Cli.Extensions;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Encrypt;

internal sealed class EncryptStringCommand(ILogger<EncryptStringCommand> logger, ILogger<SecretLogger> secretLogger)
    : BaseCommand(logger)
{
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;

    public override void Validate()
    {
        Parameters.ShouldContainSingleParameter();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override Task ExecuteAsync()
    {
        var algorithm = Options.GetOption<AlgorithmOption>()?.Value ?? AlgorithmTokens.Aes.Medium;
        _secretLogger.LogInformation("Encrypted value: {value}", algorithm);
        return Task.CompletedTask;
    }
}