namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionNameNotParsedException(string name) : IntentionalException($"Unable to parse option name '{name}'")
{
    
}