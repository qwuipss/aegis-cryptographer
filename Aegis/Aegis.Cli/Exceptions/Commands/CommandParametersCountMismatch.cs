using Aegis.Cli.Commands;

namespace Aegis.Cli.Exceptions.Commands;

internal sealed class CommandParametersCountMismatch(int expected, int actual)
    : IntentionalException($"Specified command expected {expected} parameter(s) but found {actual}")
{
}