using System.Collections.Immutable;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Aegis.Core.Algorithms;

/// <summary>
/// Advanced Encryption Standard in Galois/Counter Mode with per-block nonce increment for stream encryption
/// </summary>
public sealed class RuneAlgorithm(ImmutableArray<byte> secret) : BaseAlgorithm
{
    private const int KeySizeBytes = 32;
    private const int SaltSizeBytes = 16;
    private const int TagSizeBytes = 16;
    private const int NonceSizeBytes = 12;
    private const int BlockSizeBytes = 2 * 1024 * 1024;

    private const int Argon2IdIterations = 6;
    private const int Argon2IdDegreeOfParallelism = 6;
    private const int Argon2IdMemorySizeKBytes = 128 * 1024;

    private readonly ImmutableArray<byte> _secret = secret;

    public override async Task EncryptAsync(Stream readStream, Stream writeStream)
    {
        ValidateStreamProperties(readStream, writeStream);

        var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        var key = GetKey(salt);
        var nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);

        var plainText = new byte[BlockSizeBytes];
        var cipherText = new byte[plainText.Length];
        var tag = new byte[TagSizeBytes];

        await writeStream.WriteAsync(salt);
        await writeStream.WriteAsync(nonce);

        using var aesGcm = new AesGcm(key, TagSizeBytes);

        var blockCount = 0;
        while (true)
        {
            var offset = blockCount * BlockSizeBytes;
            var bytesRead = await readStream.ReadAsync(plainText);
            if (bytesRead is 0)
            {
                break;
            }

            aesGcm.Encrypt(nonce, plainText, cipherText, tag);

            await writeStream.WriteAsync(cipherText.AsMemory(0, bytesRead));
            blockCount++;

            IncrementNonce(nonce);
        }
    }

    public override Task DecryptAsync(Stream readStream, Stream writeStream)
    {
        throw new NotImplementedException();
    }

    private static void IncrementNonce(byte[] nonce)
    {
        for (var i = nonce.Length - 1; i >= 0; i--)
        {
            if (++nonce[i] is not 0)
                break;
        }
    }

    private byte[] GetKey(byte[] salt)
    {
        using var argon = new Argon2id(_secret.ToArray())
        {
            Iterations = Argon2IdIterations,
            DegreeOfParallelism = Argon2IdDegreeOfParallelism,
            MemorySize = Argon2IdMemorySizeKBytes,
            Salt = salt,
        };
        var key = argon.GetBytes(KeySizeBytes);
        return key;
    }
}