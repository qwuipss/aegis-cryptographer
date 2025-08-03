using System.Collections.Immutable;

namespace Aegis.Core.Algorithms;

public interface IAlgorithm
{
    byte[] Encrypt(ImmutableArray<byte> data);
    byte[] Decrypt(ImmutableArray<byte> data);
}