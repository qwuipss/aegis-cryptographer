using System.Collections.Immutable;
using System.Security.Cryptography;

namespace Aegis.Core.Algorithms;

public sealed class Aes0Algorithm : IAlgorithm
{
    private const int KeySizeBytes = 32;
    private const int TagSizeBytes = 16;
    private const int IvSizeBytes = 12;

    private readonly byte[] _key;
    private readonly byte[] _iv;

    public Aes0Algorithm(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        _key = pbkdf2.GetBytes(KeySizeBytes);
        _iv = RandomNumberGenerator.GetBytes(IvSizeBytes);
    }

    public byte[] Encrypt(ImmutableArray<byte> data)
    {
        byte[] plaintext = data.ToArray();
        byte[] ciphertext = new byte[plaintext.Length];
        byte[] tag = new byte[TagSizeBytes];
        using var aesGcm = new AesGcm(_key);
        aesGcm.Encrypt(_iv, plaintext, ciphertext, tag);

        return _iv.Concat(tag).Concat(ciphertext).ToArray();
    }

    public byte[] Decrypt(ImmutableArray<byte> data)
    {
        byte[] fullData = data.ToArray();

        if (fullData.Length < IvSizeBytes + TagSizeBytes)
            throw new ArgumentException("Invalid ciphertext format.");

        byte[] iv = fullData.AsSpan(0, IvSizeBytes).ToArray();
        byte[] tag = fullData.AsSpan(IvSizeBytes, TagSizeBytes).ToArray();
        byte[] ciphertext = fullData.AsSpan(IvSizeBytes + TagSizeBytes).ToArray();
        byte[] plaintext = new byte[ciphertext.Length];

        using var aesGcm = new AesGcm(_key);
        aesGcm.Decrypt(iv, ciphertext, tag, plaintext);

        return plaintext;
    }
}