namespace Aegis.Cli.Exceptions.Parsers.Commands;

internal sealed class CommandNotRecognizedException : IntentionalCliException
{
    public CommandNotRecognizedException() : base("Unable to resolve command. Command is not recognized")
    {
    }

    public CommandNotRecognizedException(string command) : base(
        $"Unable to resolve command '{command}'. Command is not recognized"
    )
    {
    }
};