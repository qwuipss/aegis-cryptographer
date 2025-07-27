using Aegis.Cli.Options;

namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionDuplicateException(OptionKey key) : IntentionalException($"Duplicate of option '{key}' detected")
{
}