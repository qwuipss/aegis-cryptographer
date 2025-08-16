using System.Text;
using Aegis.Core.Algorithms;
using Aegis.Core.Services;
using FluentAssertions;

namespace Aegis.Core.Tests.Algorithms;

[TestFixture]
public sealed class RuneAlgorithm_Tests
{
    [Test]
    public async Task AS()
    {
        var algorithm = new RuneAlgorithm([.."secret"u8.ToArray(),], new CryptoService());

        var message = new byte[1024 * 1024 * 6];
        Random.Shared.NextBytes(message);
        var readStream = new MemoryStream([..message.ToArray(),]);
        var writeStream = new MemoryStream();

        await algorithm.EncryptAsync(readStream, writeStream);

        (readStream, writeStream) = (writeStream, readStream);
        readStream.Position = 0;
        writeStream.Position = 0;

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedMessage = writeStream.ToArray();
        decryptedMessage.Should().Equal(message);
    }
}