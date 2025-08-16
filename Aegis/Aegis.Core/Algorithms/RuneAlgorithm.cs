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

        if (readStream.Length is 0)
        {
            throw new UnexpectedEndOfStreamException(nameof(readStream));
        }

        var salt = _cryptoService.GetRandomBytes(SaltSizeBytes);
        var nonce = _cryptoService.GetRandomBytes(NonceSizeBytes);
        var tag = new byte[TagSizeBytes];
        var plainText = readStream.Length > BlockSizeBytes ?  new byte[BlockSizeBytes] : new byte[readStream.Length];
        var cipherText = new byte[plainText.Length];

        await writeStream.WriteAsync(salt);
        await writeStream.WriteAsync(nonce);

        var key = GetKey(salt);
        using var aesGcm = new AesGcm(key, TagSizeBytes);

        while (true)
        {
            var bytesRead = await readStream.ReadAsync(plainText);
            var isFullBlockRead = bytesRead == plainText.Length;
            if (isFullBlockRead)
            {
                aesGcm.Encrypt(nonce, plainText, cipherText, tag);
            }
            else
            {
                aesGcm.Encrypt(nonce, plainText.AsSpan(..bytesRead), cipherText.AsSpan(..bytesRead), tag);
            }

            await writeStream.WriteAsync(tag);
            await writeStream.WriteAsync(isFullBlockRead ? cipherText : cipherText.AsMemory(..bytesRead));

            if (readStream.Position == readStream.Length)
            {
                break;
            }

            IncrementNonce(nonce);
        }
    }

    public override async Task DecryptAsync(Stream readStream, Stream writeStream)
    {
        ValidateStreamProperties(readStream, writeStream);

        const int readStreamMinLength = SaltSizeBytes + NonceSizeBytes + TagSizeBytes;
        if (readStream.Length <= readStreamMinLength)
        {
            throw new UnexpectedEndOfStreamException(nameof(readStream));
        }

        var salt = new byte[SaltSizeBytes];
        var nonce = new byte[NonceSizeBytes];
        var remainLength = readStream.Length - readStreamMinLength;
        var cipherText = remainLength > BlockSizeBytes ? new byte[BlockSizeBytes] : new byte[remainLength];
        var plainText = new byte[cipherText.Length];
        var tag = new byte[TagSizeBytes];

        await readStream.ReadExactlyAsync(salt);
        await readStream.ReadExactlyAsync(nonce);

        var key = GetKey(salt);
        using var aesGcm = new AesGcm(key, TagSizeBytes);

        while (true)
        {
            await readStream.ReadExactlyAsync(tag);

            var bytesRead = await readStream.ReadAsync(cipherText);
            var isFullBlockRead = bytesRead == plainText.Length;
            if (isFullBlockRead)
            {
                aesGcm.Decrypt(nonce, cipherText, tag, plainText);
            }
            else
            {
                aesGcm.Decrypt(nonce, cipherText.AsSpan(..bytesRead), tag, plainText.AsSpan(..bytesRead));
            }

            await writeStream.WriteAsync(isFullBlockRead ? plainText : plainText.AsMemory(0, bytesRead));

            if (readStream.Position == readStream.Length)
            {
                break;
            }

            if (readStream.Length - readStream.Position < TagSizeBytes)
            {
                throw new UnexpectedEndOfStreamException(nameof(readStream));
            }

            IncrementNonce(nonce);
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