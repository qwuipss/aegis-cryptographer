using Aegis.Cli.Extensions;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Logging;
using Aegis.Core.Algorithms;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Decrypt;

internal sealed class DecryptStringCommand(
    ILogger<DecryptStringCommand> logger,
    ILogger<SecretLogger> secretLogger,
    IAlgorithmResolver algorithmResolver
)
    : BaseCommand(logger)
{
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;
    private readonly IAlgorithmResolver _algorithmResolver = algorithmResolver;

    public override void Validate()
    {
        Parameters.ShouldNotContainParameters();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override async Task ExecuteAsync()
    {
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value ?? AlgorithmTokens.Rune;
        var algorithm = _algorithmResolver.Resolve(algorithmToken);

        var r = Convert.FromBase64String("fBzyhri96/8r5CPG2uLkTMKQibDXifecnJi82UhjHYCxl8HGNTkYFJX3UIc3PY23MbVgYg+a9w==");
        var w = new MemoryStream();
        await algorithm.DecryptAsync(new MemoryStream(r), w);

        var t = w.ToArray();

        _secretLogger.LogInformation("Decrypted string: {value}", Globals.ConsoleEncoding.GetString(t));
    }
}