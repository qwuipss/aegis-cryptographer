using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Exceptions.Parsers;
using Aegis.Cli.Parsers.Decrypt;
using Aegis.Cli.Parsers.Encrypt;
using Aegis.Cli.Parsers.Factory;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers;

internal sealed class RootCommandParser(ILogger<RootCommandParser> logger, ICommandParsersFactory parserFactory) : ICommandParser
{
    private readonly ILogger<RootCommandParser> _logger = logger;
    private readonly ICommandParsersFactory _parserFactory = parserFactory;

    public ICommand Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length < index + 1)
        {
            throw new TokenExpectedException();
        }

        var token = args[index];
        ICommandParser parser = token switch
        {
            Tokens.Encrypt.LongToken or Tokens.Encrypt.ShortToken => _parserFactory.Create<EncryptCommandParser>(),
            Tokens.Decrypt.LongToken or Tokens.Decrypt.ShortToken => _parserFactory.Create<DecryptCommandParser>(),
            _ => throw new UnexpectedTokenException(token),
        };

        _logger.LogDebug("Resolved parser '{parserType}'", parser.GetType().Name);

        var command = parser.Parse(args, 1);

        _logger.LogDebug("Parsed command '{commandName}'", command.GetType().Name);

        return command;
    }
}