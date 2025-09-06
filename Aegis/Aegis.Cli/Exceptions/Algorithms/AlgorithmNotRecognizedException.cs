namespace Aegis.Cli.Exceptions.Algorithms;

internal sealed class AlgorithmNotRecognizedException(string algorithm) : IntentionalCliException(
    $"Unable to resolve algorithm '{algorithm}'. Algorithm is not recognized"
)
{
}