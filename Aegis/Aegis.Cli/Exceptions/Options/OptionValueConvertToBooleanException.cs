namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueConvertToBooleanException(string value) : IntentionalException($"Unable to convert '{value}' to a boolean")
{
    
}