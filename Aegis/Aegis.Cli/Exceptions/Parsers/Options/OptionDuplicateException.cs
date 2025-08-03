using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionDuplicateException(IOption key) : IntentionalException($"Duplicate of option '{key.GetType().Name}' detected")
{
}