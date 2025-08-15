using System.Collections.Immutable;
using System.Security.Cryptography;
using Aegis.Core.Exceptions.Algorithms;
using Aegis.Core.Services;
using Konscious.Security.Cryptography;

namespace Aegis.Core.Algorithms;

/// <summary>
/// Advanced Encryption Standard in Galois/Counter Mode with per-block nonce increment for stream encryption and Argon2Id based key
/// </summary>
public sealed class RuneAlgorithm(ImmutableArray<byte> secret, ICryptoService cryptoService) : BaseAlgorithm
{
    private const int KeySizeBytes = 32;
    private const int SaltSizeBytes = 16;
    private const int TagSizeBytes = 16;
    private const int NonceSizeBytes = 12;
    private const int BlockSizeBytes = 2 * 1024 * 1024;

    private const int Argon2IdIterations = 6;
    private const int Argon2IdDegreeOfParallelism = 6;
    private const int Argon2IdMemorySizeKBytes = 128 * 1024;

    private readonly ICryptoService _cryptoService = cryptoService;
    private readonly ImmutableArray<byte> _secret = secret;

    public override async Task EncryptAsync(Stream readStream, Stream writeStream)
    {
        ValidateStreamProperties(readStream, writeStream);

        var salt = _cryptoService.GetRandomBytes(SaltSizeBytes);
        var nonce = _cryptoService.GetRandomBytes(NonceSizeBytes);

        var remainLength = readStream.Length - readStream.Position;
        if (remainLength is 0)
        {
            throw new UnexpectedEndOfStreamException(nameof(readStream));
        }

        var plainText = remainLength > BlockSizeBytes ? new byte[BlockSizeBytes] : new byte[readStream.Length];
        var tag = new byte[TagSizeBytes];

        await writeStream.WriteAsync(salt);
        await writeStream.WriteAsync(nonce);

        var key = GetKey(salt);
        using var aesGcm = new AesGcm(key, TagSizeBytes);

        while (true)
        {
            var bytesRead = await readStream.ReadAsync(plainText);
            if (bytesRead is 0)
            {
                break;
            }

            plainText = plainText[..bytesRead];
            var cipherText = new byte[plainText.Length];
            aesGcm.Encrypt(nonce, plainText, cipherText, tag);

            await writeStream.WriteAsync(tag);
            await writeStream.WriteAsync(cipherText.AsMemory(0, bytesRead));

            IncrementNonce(nonce);
        }
    }

    public override async Task DecryptAsync(Stream readStream, Stream writeStream)
    {
        ValidateStreamProperties(readStream, writeStream);

        var salt = new byte[SaltSizeBytes];
        var bytesRead = await readStream.ReadAsync(salt);

        if (bytesRead < SaltSizeBytes)
        {
            throw new Exception();
        }

        var key = GetKey(salt);
        var nonce = new byte[NonceSizeBytes];

        bytesRead = await readStream.ReadAsync(nonce);
        if (bytesRead < NonceSizeBytes)
        {
            throw new Exception();
        }

        var cipherText = new byte[BlockSizeBytes];
        var tag = new byte[TagSizeBytes];

        using var aesGcm = new AesGcm(key, TagSizeBytes);

        while (true)
        {
            bytesRead = await readStream.ReadAsync(tag);
            if (bytesRead is not TagSizeBytes)
            {
                throw new Exception();
            }

            bytesRead = await readStream.ReadAsync(cipherText.AsMemory(0, bytesRead));
            if (bytesRead is 0)
            {
                break;
            }

            var plainText = new byte[bytesRead];
            cipherText = cipherText[..bytesRead];
            aesGcm.Decrypt(nonce, cipherText, tag, plainText);

            await writeStream.WriteAsync(cipherText.AsMemory(0, bytesRead));

            DecrementNonce(nonce);
        }
    }

    private static void IncrementNonce(byte[] nonce)
    {
        for (var i = nonce.Length - 1; i >= 0; i--)
        {
            if (++nonce[i] is not 0)
            {
                break;
            }
        }
    }

    private static void DecrementNonce(byte[] nonce)
    {
        for (var i = nonce.Length - 1; i >= 0; i--)
        {
            if (nonce[i]-- is not 0)
            {
                break;
            }
        }
    }

    private byte[] GetKey(byte[] salt)
    {
        return _cryptoService.GetArgon2IdKey(
            _secret.ToArray(),
            salt,
            KeySizeBytes,
            Argon2IdIterations,
            Argon2IdDegreeOfParallelism,
            Argon2IdMemorySizeKBytes
        );
    }
}