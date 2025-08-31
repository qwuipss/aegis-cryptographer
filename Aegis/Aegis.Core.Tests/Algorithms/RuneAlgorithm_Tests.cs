using Aegis.Core.Algorithms;
using Aegis.Core.Services;
using FluentAssertions;
using NSubstitute;

namespace Aegis.Core.Tests.Algorithms;

internal sealed class RuneAlgorithm_Tests
{
    private ICryptoService _cryptoService;

    [SetUp]
    public void Setup()
    {
        _cryptoService = Substitute.For<ICryptoService>();

        _cryptoService
            .GetRandomBytes(0)
            .ReturnsForAnyArgs(callInfo =>
                {
                    var random = new Random(1);
                    var bytes = new byte[callInfo.ArgAt<int>(0)];
                    random.NextBytes(bytes);
                    return bytes;
                }
            );

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
    [TestCase(174_080)]
    [TestCase(174_081)]
    [TestCase(562_567)]
    [TestCase(1_022_521)]
    [TestCase(1_228_799)]
    [TestCase(1_228_800)]
    [TestCase(1_228_801)]
    [TestCase(3_500_000)]
    [TestCase(8_500_100)]
    [TestCase(16_000_000)]
    public async Task Should_correctly_encrypt_and_decrypt(int stringSize)
    {
        var random = new Random(1);
        var algorithm = new RuneAlgorithm([], _cryptoService);
        var stringToEncrypt = new byte[stringSize];
        random.NextBytes(stringToEncrypt);

        var readStream = new MemoryStream(stringToEncrypt);
        var writeStream = new MemoryStream();

        await algorithm.EncryptAsync(readStream, writeStream);

        (readStream.Position, writeStream.Position) = (0, 0);
        (readStream, writeStream) = (writeStream, readStream);

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedString = writeStream.ToArray();
        decryptedString.Should().Equal(stringToEncrypt);
    }

    [TestCase(
        new byte[] { 0, 1, 2, 3, 4, 5, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 253, 43, 95, 5, 207, 31, 224, 226, 128, 210, 91, 247, 252, 139, 119, 127, 78, 241, 11, 253, 199,
            230,
        }
    )]
    [TestCase(
        new byte[] { 5, 4, 3, 2, 1, 0, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 94, 49, 215, 24, 122, 99, 165, 27, 93, 122, 243, 246, 218, 34, 28, 123, 75, 244, 10, 252, 194, 227,
        }
    )]
    [TestCase(
        new byte[] { 1, 1, 1, 1, 1, 1, 1, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 184, 169, 190, 9, 43, 249, 43, 14, 44, 196, 213, 249, 137, 249, 115, 115, 79, 241, 8, 255, 194,
            226, 167,
        }
    )]
    [TestCase(
        new byte[] { 1, 2, 4, 7, 1, 4, 6, 2, 3, 8, 6, 0, 4, 7, 3, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 120, 239, 115, 126, 14, 197, 124, 17, 2, 101, 219, 100, 232, 178, 83, 98, 79, 242, 13, 249, 194,
            231, 160, 210, 119, 88, 66, 20, 37, 203, 151
        }
    )]
    public async Task Should_correctly_encrypt(byte[] stringToEncrypt, byte[] expectedEncryptedString)
    {
        var algorithm = new RuneAlgorithm([], _cryptoService);

        var readStream = new MemoryStream(stringToEncrypt);
        var writeStream = new MemoryStream();

        await algorithm.EncryptAsync(readStream, writeStream);

        var encryptedString = writeStream.ToArray();
        encryptedString.Should().Equal(expectedEncryptedString);
    }

    [TestCase(
        new byte[] { 0, 1, 2, 3, 4, 5, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 253, 43, 95, 5, 207, 31, 224, 226, 128, 210, 91, 247, 252, 139, 119, 127, 78, 241, 11, 253, 199,
            230,
        }
    )]
    [TestCase(
        new byte[] { 5, 4, 3, 2, 1, 0, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 94, 49, 215, 24, 122, 99, 165, 27, 93, 122, 243, 246, 218, 34, 28, 123, 75, 244, 10, 252, 194, 227,
        }
    )]
    [TestCase(
        new byte[] { 1, 1, 1, 1, 1, 1, 1, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 184, 169, 190, 9, 43, 249, 43, 14, 44, 196, 213, 249, 137, 249, 115, 115, 79, 241, 8, 255, 194,
            226, 167,
        }
    )]
    [TestCase(
        new byte[] { 1, 2, 4, 7, 1, 4, 6, 2, 3, 8, 6, 0, 4, 7, 3, },
        new byte[]
        {
            70, 208, 134, 130, 64, 151, 228, 163, 149, 207, 255, 70, 105, 156, 115, 196, 70, 208, 134, 130, 64, 151, 228, 163,
            149, 207, 255, 70, 120, 239, 115, 126, 14, 197, 124, 17, 2, 101, 219, 100, 232, 178, 83, 98, 79, 242, 13, 249, 194,
            231, 160, 210, 119, 88, 66, 20, 37, 203, 151
        }
    )]
    public async Task Should_correctly_decrypt(byte[] expectedDecryptedString, byte[] encryptedString)
    {
        var algorithm = new RuneAlgorithm([], _cryptoService);

        var readStream = new MemoryStream(encryptedString);
        var writeStream = new MemoryStream();

        await algorithm.DecryptAsync(readStream, writeStream);

        var decryptedString = writeStream.ToArray();
        decryptedString.Should().Equal(expectedDecryptedString);
    }
}