using Aegis.Core.Algorithms;
using Aegis.Core.Services;
using FluentAssertions;
using NSubstitute;

namespace Aegis.Core.Tests.Algorithms;

[TestFixture]
public sealed class RuneAlgorithm_Tests
{
    private ICryptoService _cryptoService;

    [SetUp]
    public void Setup()
    {
        _cryptoService = Substitute.For<ICryptoService>();
        // _cryptoService.GetRandomBytes(0).Returns(new );
        _cryptoService
            .GetArgon2IdKey(null!, null!, 0, 0, 0, 0)
            .ReturnsForAnyArgs(Enumerable.Range(0, 32).Select(x => (byte)x).ToArray());
    }

    [TestCase(1)]
    [TestCase(120)]
    [TestCase(500)]
    [TestCase(1024)]
    [TestCase(1362)]
    [TestCase(2048)]
    [TestCase(2049)]
    [TestCase(4096)]
    [TestCase(4097)]
    [TestCase(10_000)]
    [TestCase(55_287)]
    [TestCase(128_682)]
    [TestCase(562_567)]
    [TestCase(1_022_521)]
    [TestCase(3_500_000)]
    [TestCase(8_500_100)]
    [TestCase(16_000_000)]
    public async Task Should_correctly_encrypt_and_decrypt(int messageSize)
    {
        var random = new Random(0);
        var algorithm = new RuneAlgorithm([0], new CryptoService());
        var messageToEncrypt = new byte[messageSize];
        random.NextBytes(messageToEncrypt);

        var readStream = new MemoryStream(messageToEncrypt);
        var writeStream = new MemoryStream();

        await algorithm.EncryptAsync(readStream, writeStream);

        (readStream.Position, writeStream.Position) = (0, 0);
        (readStream, writeStream) = (writeStream, readStream);

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedMessage = writeStream.ToArray();
        decryptedMessage.Should().Equal(messageToEncrypt);
    }
}