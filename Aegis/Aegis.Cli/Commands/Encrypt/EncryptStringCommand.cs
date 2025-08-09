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
        Parameters.ShouldNotContainParameters();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override async Task ExecuteAsync()
    {
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value ?? AlgorithmTokens.Rune;
        var algorithm = _algorithmResolver.Resolve(algorithmToken);

        var x = Globals.ConsoleEncoding.GetBytes("hello world");
        var w = new MemoryStream();
        await algorithm.EncryptAsync(new MemoryStream(x), w);
        
        var t = w.ToArray();
        
        _secretLogger.LogInformation("Encrypted string: {value}", Convert.ToBase64String(t));
    }
}