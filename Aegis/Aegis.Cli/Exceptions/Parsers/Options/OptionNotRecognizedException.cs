namespace Aegis.Cli.Exceptions.Parsers.Options;

internal sealed class OptionNotRecognizedException(string option) : IntentionalCliException($"Unable to resolve option '{option}'. Option is not recognized")
{
}