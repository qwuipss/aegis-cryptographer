namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionNotRecognizedException(string option) : IntentionalCliException($"Unable to parse option '{option}'. Option is not recognized")
{
}