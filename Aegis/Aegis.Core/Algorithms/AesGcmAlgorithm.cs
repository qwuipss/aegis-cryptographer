using System.Collections.Immutable;

namespace Aegis.Core.Algorithms;

internal sealed class AesGcmAlgorithm : IAlgorithm
{
    public byte[] Encrypt(ImmutableArray<byte> data)
    {
        return [];
    }
}