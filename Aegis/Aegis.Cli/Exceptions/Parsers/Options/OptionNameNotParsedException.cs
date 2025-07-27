namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionNameNotParsedException(string name) : IntentionalException($"Unable to parse option name '{name}'")
{
    
}