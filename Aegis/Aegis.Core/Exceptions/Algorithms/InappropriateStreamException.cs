namespace Aegis.Core.Exceptions.Algorithms;

internal sealed class InappropriateStreamException(string stream) : IntentionalCoreException($"Inappropriate stream '{stream}' specified")
{
}