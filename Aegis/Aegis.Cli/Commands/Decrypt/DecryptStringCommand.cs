using System.Text;
using Aegis.Cli.Extensions;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Services.Logging;
using Aegis.Core.Algorithms;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Decrypt;

internal sealed class DecryptStringCommand(
    ILogger<DecryptStringCommand> logger,
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
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value ?? AlgorithmTokens.Rune;
        var algorithm = _algorithmResolver.Resolve(algorithmToken);
        var messageToDecrypt = _consoleReader.ReadSecret();
        // var base64MessageToDecrypt = Convert.Base(messageToDecrypt);
        var readStream = new MemoryStream(messageToDecrypt);
        var writeStream = new MemoryStream();

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedMessage = Globals.ConsoleEncoding.GetString(writeStream.ToArray());

        _secretLogger.LogInformation("Decrypted string: {value}", decryptedMessage);
    }
}