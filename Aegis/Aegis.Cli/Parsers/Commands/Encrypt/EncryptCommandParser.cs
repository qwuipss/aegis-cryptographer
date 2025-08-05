using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Encrypt;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Exceptions.Parsers.Commands;
using Aegis.Cli.Parsers.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands.Encrypt;

internal sealed class EncryptCommandParser(ILogger<EncryptCommandParser> logger, ICommandFactory commandFactory, IOptionsParser optionsParser)
    : BaseCommandParser(logger, optionsParser)
{
    private readonly ICommandFactory _commandFactory = commandFactory;

    public override ICommand Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length <= index)
        {
            throw new CommandNotRecognizedException();
        }

        var token = args[index];
        var command = token switch
        {
            CommandTokens.Common.String.LongToken or CommandTokens.Common.String.ShortToken => GetEncryptStringCommand(args, index + 1),
            _ => throw new CommandNotRecognizedException(),
        };

        return command;
    }

    private EncryptStringCommand GetEncryptStringCommand(ImmutableArray<string> args, int index)
    {
        var (options, parametersIndex) = ParseOptions(args, index);
        return _commandFactory.Create<EncryptStringCommand>(args[parametersIndex..], options);
    }
}