using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli.Parsers.Factory;

internal sealed class CommandParserFactory(IServiceProvider serviceProvider) : ICommandParsersFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TParser Create<TParser>() where TParser : ICommandParser
    {
        return _serviceProvider.GetRequiredService<TParser>();
    }
}