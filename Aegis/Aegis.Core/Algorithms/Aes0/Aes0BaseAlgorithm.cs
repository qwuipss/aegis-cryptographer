using System.Collections.Immutable;
using System.Security.Cryptography;

namespace Aegis.Core.Algorithms.Aes0;

public abstract class Aes0BaseAlgorithm : IAlgorithm
{
    private const int IvSize = 12;
    private const int KeySize = 32;

    private readonly byte[] _key;
    private readonly byte[] _salt;
    private readonly byte[] _iv;
    
    private readonly int _tagSize;

    public Aes0BaseAlgorithm(string secret, int saltSize, int tagSize)
    {
        _salt = RandomNumberGenerator.GetBytes(saltSize);
        using var pbkdf2 = new Rfc2898DeriveBytes(secret, _salt, 100_000, HashAlgorithmName.SHA256);
        _key = pbkdf2.GetBytes(KeySize);
        _iv = RandomNumberGenerator.GetBytes(IvSize);
        _tagSize = tagSize;
    }

    public byte[] Encrypt(ImmutableArray<byte> data)
    {
        // var plaintext = data.ToArray();
        // var ciphertext = new byte[plaintext.Length];
        // var tag = new byte[TagSizeBytes];
        // using var aesGcm = new AesGcm(_key);
        // aesGcm.Encrypt(_iv, plaintext, ciphertext, tag);
        //
        // return _iv.Concat(tag).Concat(ciphertext).ToArray();
        return [];
    }

    public byte[] Decrypt(ImmutableArray<byte> data)
    {
        // var fullData = data.ToArray();
        //
        // if (fullData.Length < IvSizeBytes + TagSizeBytes)
        //     throw new ArgumentException("Invalid ciphertext format.");
        //
        // var iv = fullData.AsSpan(0, IvSizeBytes).ToArray();
        // var tag = fullData.AsSpan(IvSizeBytes, TagSizeBytes).ToArray();
        // var ciphertext = fullData.AsSpan(IvSizeBytes + TagSizeBytes).ToArray();
        // var plaintext = new byte[ciphertext.Length];
        //
        // using var aesGcm = new AesGcm(_key);
        // aesGcm.Decrypt(iv, ciphertext, tag, plaintext);
        //
        // return plaintext;
        return [];
    }
}