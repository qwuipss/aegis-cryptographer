namespace Aegis.Cli.Parsers.Factory;

internal interface ICommandParsersFactory
{
    TParser Create<TParser>()
        where TParser : ICommandParser;
}