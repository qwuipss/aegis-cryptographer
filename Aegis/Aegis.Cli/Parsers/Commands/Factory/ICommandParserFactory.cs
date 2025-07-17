namespace Aegis.Cli.Parsers.Commands.Factory;

internal interface ICommandParserFactory
{
    TParser Create<TParser>()
        where TParser : ICommandParser;
}