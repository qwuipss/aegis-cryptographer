namespace Aegis.Core.Algorithms;

public interface IAlgorithm
{
    Task EncryptAsync(Stream readStream, Stream writeStream);

    Task DecryptAsync(Stream readStream, Stream writeStream);
}