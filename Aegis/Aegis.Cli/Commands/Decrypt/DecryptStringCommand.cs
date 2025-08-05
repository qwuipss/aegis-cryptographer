using Aegis.Cli.Extensions;
using Aegis.Cli.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Decrypt;

internal sealed class DecryptStringCommand(ILogger<DecryptStringCommand> logger, ILogger<SecretLogger> secretLogger)
    : BaseCommand(logger)
{
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;

    public override void Validate()
    {
        Parameters.ShouldNotContainParameters();
    }

    public override Task ExecuteAsync()
    {
        _secretLogger.LogInformation("Decrypted value: {value}", "12312312312   sd");
        return Task.CompletedTask;
    }
}