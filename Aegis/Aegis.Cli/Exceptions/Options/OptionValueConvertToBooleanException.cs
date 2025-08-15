namespace Aegis.Cli.Exceptions.Options;

internal sealed class OptionValueConvertToBooleanException(string value) : IntentionalCliException($"Unable to convert '{value}' to a boolean")
{
    
}