using System.Collections.Immutable;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Aegis.Core.Algorithms.AesGcm;

public class AesGcmAlgorithm : IAlgorithm
{
    private const int KeySize = 32;
    private const int SaltSize = 16;
    private const int TagSize = 16;
    private const int NonceSize = 12;

    private readonly byte[] _salt;
    private readonly byte[] _key;
    private readonly byte[] _nonce;

    public AesGcmAlgorithm(ImmutableArray<byte> secret)
    {
        _salt = RandomNumberGenerator.GetBytes(SaltSize);
        using var argon = new Argon2id(secret.ToArray())
        {
            Iterations = 6,
            DegreeOfParallelism = 6,
            MemorySize = 128 * 1024,
            Salt = _salt,
        };
        _key = argon.GetBytes(KeySize);
        _nonce = RandomNumberGenerator.GetBytes(NonceSize);
    }

    public byte[] Encrypt(ImmutableArray<byte> data)
    {
        var plainText = data.ToArray();
        var cipherText = new byte[plainText.Length];
        var tag = new byte[TagSize];
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);

        using var aesGcm = new System.Security.Cryptography.AesGcm(_key, TagSize);
        aesGcm.Encrypt(_nonce, plainText, cipherText, tag);
        // aesGcm.Decrypt(_nonce, cipherText, tag, plainText);

        var result = new byte[_salt.Length + nonce.Length + cipherText.Length + tag.Length];
        Buffer.BlockCopy(_salt, 0, result, 0, _salt.Length);
        Buffer.BlockCopy(nonce, 0, result, _salt.Length, nonce.Length);
        Buffer.BlockCopy(cipherText, 0, result, _salt.Length + nonce.Length, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, _salt.Length + nonce.Length + cipherText.Length, tag.Length);

        return result;
    }
    public async Task<byte[]> Encrypt(Stream stream)
    {
        // var nonce = stream.ReadAsync();
        
        var plainText = data.ToArray();
        var cipherText = new byte[plainText.Length];
        var tag = new byte[TagSize];
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);

        using var aesGcm = new System.Security.Cryptography.AesGcm(_key, TagSize);
        aesGcm.Encrypt(_nonce, plainText, cipherText, tag);
        // aesGcm.Decrypt(_nonce, cipherText, tag, plainText);

        var result = new byte[_salt.Length + nonce.Length + cipherText.Length + tag.Length];
        Buffer.BlockCopy(_salt, 0, result, 0, _salt.Length);
        Buffer.BlockCopy(nonce, 0, result, _salt.Length, nonce.Length);
        Buffer.BlockCopy(cipherText, 0, result, _salt.Length + nonce.Length, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, _salt.Length + nonce.Length + cipherText.Length, tag.Length);

        return result;
    }
    public async Task<byte[]> Encrypt(Stream stream)
    {
        // var nonce = stream.ReadAsync();
        
        var plainText = data.ToArray();
        var cipherText = new byte[plainText.Length];
        var tag = new byte[TagSize];
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);

        using var aesGcm = new System.Security.Cryptography.AesGcm(_key, TagSize);
        aesGcm.Encrypt(_nonce, plainText, cipherText, tag);
        // aesGcm.Decrypt(_nonce, cipherText, tag, plainText);

        var result = new byte[_salt.Length + nonce.Length + cipherText.Length + tag.Length];
        Buffer.BlockCopy(_salt, 0, result, 0, _salt.Length);
        Buffer.BlockCopy(nonce, 0, result, _salt.Length, nonce.Length);
        Buffer.BlockCopy(cipherText, 0, result, _salt.Length + nonce.Length, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, _salt.Length + nonce.Length + cipherText.Length, tag.Length);

        return result;
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