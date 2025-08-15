namespace Aegis.Cli.Exceptions.Parsers.Commands;

internal sealed class CommandNotRecognizedException() : IntentionalCliException("Unable to parse command. Command is not recognized");