using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Encrypt;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Exceptions.Parsers;
using Aegis.Cli.Parsers.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands.Encrypt;

internal sealed class EncryptCommandParser(ILogger<EncryptCommandParser> logger, ICommandFactory commandFactory, IOptionsParser optionsParser)
    : ICommandParser
{
    private readonly ILogger<EncryptCommandParser> _logger = logger;
    private readonly IOptionsParser _optionsParser = optionsParser;
    private readonly ICommandFactory _commandFactory = commandFactory;

    public ICommand Parse(ImmutableArray<string> parameters, int index)
    {
        if (parameters.Length < index + 1)
        {
            throw new CommandNotParsedException();
        }

        var token = parameters[index];
        var commandType = token switch
        {
            CommandTokens.Common.String.LongToken or CommandTokens.Common.String.ShortToken => GetEncryptStringCommand(parameters, index + 1),
            _ => throw new UnexpectedTokenException(token),
        };

        return commandType;
    }

    private EncryptStringCommand GetEncryptStringCommand(ImmutableArray<string> parameters, int index)
    {
        if (parameters.Length < index + 1)
        {
            throw new CommandParametersNotParsedException();
        }

        var valueToEncrypt = parameters[index];
        var options = _optionsParser.Parse(parameters, index + 2);

        return _commandFactory.Create<EncryptStringCommand>([valueToEncrypt,], options);
    }
}