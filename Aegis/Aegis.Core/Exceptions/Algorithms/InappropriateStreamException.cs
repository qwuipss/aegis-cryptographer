namespace Aegis.Core.Exceptions.Algorithms;

internal sealed class InappropriateStreamException(string stream) : Exception($"Inappropriate stream '{stream}' specified")
{
}