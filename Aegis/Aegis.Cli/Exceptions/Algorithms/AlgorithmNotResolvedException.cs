namespace Aegis.Cli.Exceptions.Algorithms;

internal sealed class AlgorithmNotResolvedException(string algorithm) : IntentionalException($"Unable to resolve algorithm '{algorithm}'")
{
    
}