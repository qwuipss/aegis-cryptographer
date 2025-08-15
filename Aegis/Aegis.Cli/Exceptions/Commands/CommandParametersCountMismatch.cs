using Aegis.Cli.Commands;

namespace Aegis.Cli.Exceptions.Commands;

internal sealed class CommandParametersCountMismatch(int expected, int actual)
    : IntentionalCliException($"Specified command expected {expected} parameter(s) but found {actual}")
{
}