namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueConvertToNumberException(string? value): IntentionalException($"Unable to convert '{value}' to a number")
{
    
}