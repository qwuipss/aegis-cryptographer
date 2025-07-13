using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Encrypt;
using Aegis.Cli.Exceptions.Parsers;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Encrypt;

internal sealed class EncryptCommandParser(ILogger<EncryptCommandParser> logger) : ICommandParser
{
    private readonly ILogger<EncryptCommandParser> _logger = logger;

    public ICommand Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length < index + 1)
        {
            throw new TokenExpectedException();
        }

        var token = args[index];
        var command = token switch
        {
            Tokens.Common.String.LongToken or Tokens.Common.String.ShortToken => new EncryptStringCommand(),
            _ => throw new UnexpectedTokenException(token),
        };

        return command;
    }
}