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
    IAlgorithmFactory algorithmFactory,
    IConsoleReader consoleReader
)
    : BaseCommand(logger)
{
    private readonly ILogger<SecretLogger> _secretLogger = secretLogger;
    private readonly IAlgorithmFactory _algorithmFactory = algorithmFactory;
    private readonly IConsoleReader _consoleReader = consoleReader;

    public override void Validate()
    {
        Parameters.ShouldNotContainParameters();
        Options.ShouldContainOnlyOptions<AlgorithmOption>();
    }

    public override async Task ExecuteAsync()
    {
        var strToDecrypt = _consoleReader.ReadSecret();
        // var 
        var algorithmToken = Options.GetOption<AlgorithmOption>()?.Value;
        var algorithm = _algorithmFactory.Create(algorithmToken);
        var readStream = strToDecrypt.ToGlobalEncodingMemoryStream();

        var writeStream = new MemoryStream();

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedStr = Globals.ConsoleEncoding.GetString(writeStream.ToArray());

        _secretLogger.LogInformation("Decrypted string: {value}", decryptedStr);
    }
}