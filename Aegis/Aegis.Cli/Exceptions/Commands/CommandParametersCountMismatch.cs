namespace Aegis.Cli.Exceptions.Commands;

internal sealed class CommandParametersCountMismatch(int expected, int actual) : IntentionalException($"Expected {expected} parameter(s) for command but found {actual}")
{
}