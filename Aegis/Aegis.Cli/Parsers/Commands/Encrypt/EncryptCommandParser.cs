using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Encrypt;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Exceptions.Parsers;
using Aegis.Cli.Exceptions.Parsers.Commands;
using Aegis.Cli.Parsers.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands.Encrypt;

internal sealed class EncryptCommandParser(ILogger<EncryptCommandParser> logger, ICommandFactory commandFactory, IOptionsParser optionsParser)
    : ICommandParser
{
    private readonly ILogger<EncryptCommandParser> _logger = logger;
    private readonly IOptionsParser _optionsParser = optionsParser;
    private readonly ICommandFactory _commandFactory = commandFactory;

    public ICommand Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length <= index)
        {
            throw new CommandNotParsedException();
        }

        var token = args[index];
        var command = token switch
        {
            CommandTokens.Common.String.LongToken or CommandTokens.Common.String.ShortToken => GetEncryptStringCommand(args, index + 1),
            _ => throw new CommandNotParsedException(),
        };

        return command;
    }

    private EncryptStringCommand GetEncryptStringCommand(ImmutableArray<string> args, int index)
    {
        var (options, parametersIndex) = _optionsParser.Parse(args, index);
        return _commandFactory.Create<EncryptStringCommand>(args[parametersIndex..], options);
    }
}