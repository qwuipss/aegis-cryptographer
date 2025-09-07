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
        var algorithmType = Options.GetAlgorithmTypeOrDefault();
        var algorithm = _algorithmFactory.Create(algorithmType);
        var stringToEncrypt = _consoleReader.ReadSecret("Enter string");
        using var readStream = stringToEncrypt.ToGlobalEncodingMemoryStream();
        var algorithmTypeSerialized = BitConverter.GetBytes((int)algorithmType);
        using var writeStream = new MemoryStream(algorithmTypeSerialized, true);

        await algorithm.EncryptAsync(readStream, writeStream);

        var encryptedStringBytes = writeStream.ToArray();
        var encryptedBase64String = Convert.ToBase64String(encryptedStringBytes);

        _secretLogger.LogInformation("Encrypted string: {value}", encryptedBase64String);
    }
}