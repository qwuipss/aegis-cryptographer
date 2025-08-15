namespace Aegis.Core.Services;

public interface ICryptoService
{
    byte[] GetRandomBytes(int length);

    byte[] GetArgon2IdKey(byte[] secret, byte[] salt, int keySizeBytes, int iterations, int degreeOfParallelism, int memorySizeKBytes);
}