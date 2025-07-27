using System.Collections.Immutable;

namespace Aegis.Core.Algorithms;

internal interface IAlgorithm
{
    byte[] Encrypt(ImmutableArray<byte> data);
}