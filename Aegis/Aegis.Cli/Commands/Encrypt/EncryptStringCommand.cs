using Aegis.Cli.Extensions;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Encrypt;

internal sealed class EncryptStringCommand(
    ILogger<EncryptStringCommand> logger,
    ILogger<SecretLogger> secretLogger,
    IAlgorithmResolver algorithmResolver,
    IConsoleReader consoleReader
)
    : BaseCommand(logger)
{
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;
    private readonly IAlgorithmResolver _algorithmResolver = algorithmResolver;
    private readonly IConsoleReader _consoleReader = consoleReader;

    public override void Validate()
    {
        Parameters.ShouldNotContainParameters();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override async Task ExecuteAsync()
    {
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value ?? nameof(Algorithm.Rune);
        var algorithm = _algorithmResolver.Resolve(algorithmToken);

        var stringToEncrypt = _consoleReader.ReadSecret();
        var readStream = stringToEncrypt.ToGlobalEncodingMemoryStream();
        var writeStream = new MemoryStream();

        await writeStream.WriteAsync([])
        await algorithm.EncryptAsync(readStream, writeStream);

        var encryptedStrBytes = writeStream.ToArray();
        var encryptedBase64Str = Convert.ToBase64String(encryptedStrBytes);

        _secretLogger.LogInformation("Encrypted string: {value}", encryptedBase64Str);
    }
}