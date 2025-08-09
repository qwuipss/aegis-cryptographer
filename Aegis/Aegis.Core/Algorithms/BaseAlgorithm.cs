using Aegis.Core.Exceptions.Algorithms;

namespace Aegis.Core.Algorithms;

public abstract class BaseAlgorithm : IAlgorithm
{
    public abstract Task EncryptAsync(Stream readStream, Stream writeStream);

    public abstract Task DecryptAsync(Stream readStream, Stream writeStream);

    protected static void ValidateStreamProperties(Stream readStream, Stream writeStream)
    {
        if (!readStream.CanRead || !readStream.CanSeek)
        {
            throw new InappropriateStreamException(nameof(readStream));
        }

        if (!writeStream.CanWrite || !writeStream.CanSeek)
        {
            throw new InappropriateStreamException(nameof(writeStream));
        }
    }
}