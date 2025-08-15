namespace Aegis.Cli.Exceptions.Algorithms;

internal sealed class AlgorithmNotResolvedException(string algorithm) : IntentionalCliException($"Unable to resolve algorithm '{algorithm}'")
{
    
}