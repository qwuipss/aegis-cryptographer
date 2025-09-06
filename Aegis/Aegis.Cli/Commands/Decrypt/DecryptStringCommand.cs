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
        // Options.ShouldContainOnlyOptions<>();
    }

    public override async Task ExecuteAsync()
    {
        var encryptedBase64String = _consoleReader.ReadSecret();
        var encryptedStringBytes = Convert.FromBase64String(encryptedBase64String);
        var algorithmTypeSerialized = encryptedStringBytes.AsSpan(0, 4).ToArray();
        var algorithmType = BitConverter.ToInt32(algorithmTypeSerialized);
        var algorithm = _algorithmFactory.Create();
        var readStream = encryptedBase64String.ToGlobalEncodingMemoryStream();
        
        
        var writeStream = new MemoryStream();

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedStr = Globals.ConsoleEncoding.GetString(writeStream.ToArray());

        _secretLogger.LogInformation("Decrypted string: {value}", decryptedStr);
    }
}