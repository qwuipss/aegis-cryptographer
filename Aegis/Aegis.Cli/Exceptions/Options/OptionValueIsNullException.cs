namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueIsNullException(string option) : IntentionalCliException($"Missing value for option '{option}'");