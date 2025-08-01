using Aegis.Cli.Extensions;
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
        Options.ShouldContainOnlyOptions();
    }

    public override Task ExecuteAsync()
    {
        _secretLogger.LogInformation("Encrypted value: {value}", "test123123123---");
        return Task.CompletedTask;
    }
}