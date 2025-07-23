namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueIsNullException(string option) : IntentionalException($"Missing value for option '{option}'");