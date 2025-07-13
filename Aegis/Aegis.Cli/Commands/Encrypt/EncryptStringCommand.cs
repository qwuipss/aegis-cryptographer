namespace Aegis.Cli.Commands.Encrypt;

internal sealed class EncryptStringCommand : ICommand
{
    public CommandResult Execute()
    {
        return new CommandResult { DisplayText = "Encrypted value: @@@", };
    }
}