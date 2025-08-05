namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionNotRecognizedException(string option) : IntentionalException($"Unable to parse option '{option}'. Option is not recognized")
{
}