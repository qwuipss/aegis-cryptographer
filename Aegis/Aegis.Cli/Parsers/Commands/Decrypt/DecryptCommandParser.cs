using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Decrypt;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Exceptions.Parsers.Commands;
using Aegis.Cli.Parsers.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands.Decrypt;

internal sealed class DecryptCommandParser(ILogger<DecryptCommandParser> logger, ICommandFactory commandFactory, IOptionsParser optionsParser)
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
            CommandTokens.Common.String.LongToken or CommandTokens.Common.String.ShortToken => GetDecryptStringCommand(args, index + 1),
            _ => throw new CommandNotRecognizedException(),
        };

        return command;
    }

    private DecryptStringCommand GetDecryptStringCommand(ImmutableArray<string> args, int index)
    {
        var (options, parametersIndex) = ParseOptions(args, index);
        return _commandFactory.Create<DecryptStringCommand>(args[parametersIndex..], options);
    }
}