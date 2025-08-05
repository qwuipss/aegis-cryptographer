namespace Aegis.Cli.Exceptions.Parsers.Commands;

internal sealed class CommandNotRecognizedException() : IntentionalException("Unable to parse command. Command is not recognized");