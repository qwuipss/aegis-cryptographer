namespace Aegis.Core.Exceptions.Algorithms;

internal sealed class UnexpectedEndOfStreamException(string stream) : IntentionalCoreException($"Unexpected end of stream '{stream}'")
{
    
}