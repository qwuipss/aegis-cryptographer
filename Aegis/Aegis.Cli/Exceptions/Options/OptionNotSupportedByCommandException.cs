using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionNotSupportedByCommandException(IOption option)
    : IntentionalException($"Option '{option.GetType().Name}' is not supported by specified command")
{
}