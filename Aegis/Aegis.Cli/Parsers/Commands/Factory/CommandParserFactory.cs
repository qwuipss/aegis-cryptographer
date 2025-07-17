using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli.Parsers.Commands.Factory;

internal sealed class CommandParserFactory(IServiceProvider serviceProvider) : ICommandParserFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TParser Create<TParser>() where TParser : ICommandParser
    {
        return _serviceProvider.GetRequiredService<TParser>();
    }
}