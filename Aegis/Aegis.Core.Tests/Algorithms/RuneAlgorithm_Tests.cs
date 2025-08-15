using Aegis.Core.Algorithms;
using NUnit.Framework;

namespace Aegis.Core.Tests.Algorithms;

[TestFixture]
public sealed class RuneAlgorithm_Tests
{
    [Test]
    public async Task Encrypt()
    {
        var algorithm = new RuneAlgorithm([.."secret"u8.ToArray(),]);

        var message = "baobab"u8;
        var readStream = new MemoryStream([..message.ToArray(),]);
        var writeStream = new MemoryStream();

        await algorithm.EncryptAsync(readStream, writeStream);

        (readStream, writeStream) = (writeStream, readStream);
Console.WriteLine();
        readStream.Position = 0;
        writeStream.Position = 0;
        
        await algorithm.DecryptAsync(readStream, writeStream);
    }
}