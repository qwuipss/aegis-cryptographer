using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Aegis.Core.Services;

public sealed class CryptoService : ICryptoService
{
    public byte[] GetRandomBytes(int length)
    {
        return RandomNumberGenerator.GetBytes(length);
    }

    public byte[] GetArgon2IdKey(byte[] secret, byte[] salt, int keySizeBytes, int iterations, int degreeOfParallelism, int memorySizeKBytes)
    {
        using var argon = new Argon2id(secret)
        {
            Iterations = iterations,
            DegreeOfParallelism = degreeOfParallelism,
            MemorySize = memorySizeKBytes,
            Salt = salt,
        };
        var key = argon.GetBytes(keySizeBytes);
        return key;
    }
}