using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionDuplicateDetectedException(IOption key) : IntentionalCliException($"Duplicate of option '{key.GetType().Name}' detected")
{
}