namespace Aegis.Core.Algorithms;

public interface IAlgorithm
{
    Task<byte[]> Encrypt(Stream readStream);

    Task<byte[]> Decrypt(Stream readStream);
}