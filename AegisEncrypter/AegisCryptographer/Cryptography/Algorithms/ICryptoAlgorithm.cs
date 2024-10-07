namespace AegisCryptographer.Cryptography.Algorithms;

public interface ICryptoAlgorithm
{
    byte[] Encrypt(byte[] data);
    byte[] Decrypt(byte[] data);
}