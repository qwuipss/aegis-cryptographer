namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueIsNullException(string name) : IntentionalException($"Missing value for option name '{name}'");