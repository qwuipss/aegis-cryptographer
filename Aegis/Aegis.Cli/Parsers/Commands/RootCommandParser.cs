using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Exceptions.Parsers.Commands;
using Aegis.Cli.Parsers.Commands.Decrypt;
using Aegis.Cli.Parsers.Commands.Encrypt;
using Aegis.Cli.Parsers.Commands.Factory;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands;

internal sealed class RootCommandParser(ILogger<RootCommandParser> logger, ICommandParserFactory parserFactory) : ICommandParser
{
    private readonly ILogger<RootCommandParser> _logger = logger;
    private readonly ICommandParserFactory _parserFactory = parserFactory;

    public ICommand Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length <= index)
        {
            throw new CommandNotParsedException();
        }

        var token = args[index];
        ICommandParser parser = token switch
        {
            CommandTokens.Encrypt.LongToken or CommandTokens.Encrypt.ShortToken => _parserFactory.Create<EncryptCommandParser>(),
            CommandTokens.Decrypt.LongToken or CommandTokens.Decrypt.ShortToken => _parserFactory.Create<DecryptCommandParser>(),
            _ => throw new CommandNotParsedException(),
        };

        _logger.LogDebug("Resolved parser '{parserType}'", parser.GetType().Name);

        var command = parser.Parse(args, 1);

        _logger.LogDebug("Parsed command '{commandName}'", command.GetType().Name);

        return command;
    }
}