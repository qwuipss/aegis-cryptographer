using System.Reflection;
using Aegis.Cli.Parsers;
using Aegis.Cli.Parsers.Factory;
using Aegis.Cli.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli.Setup;

internal static class SetupServices
{
    public static IServiceCollection SetupUtilityServices(this IServiceCollection services)
    {
        services.AddSingleton<IRunner, Runner>();
        services.AddSingleton<IOldLogFilesCleaner, OldLogFilesCleaner>();
        services.AddCommandParsers();

        return services;
    }

    private static IServiceCollection AddCommandParsers(this IServiceCollection services)
    {
        services.AddSingleton<ICommandParsersFactory, CommandParserFactory>();
        services.AddSingleton<ICommandParser, RootCommandParser>();

        var parserType = typeof(ICommandParser);
        var types = parserType
                    .Assembly
                    .GetTypes()
                    .Where(type =>
                               type != typeof(RootCommandParser)
                               && parserType.IsAssignableFrom(type)
                               && type is { IsInterface: false, IsAbstract: false, }
                    );

        foreach (var type in types)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}