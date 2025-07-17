using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Parsers.Commands;
using Aegis.Cli.Parsers.Commands.Factory;
using Aegis.Cli.Parsers.Options;
using Aegis.Cli.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli.Setup;

internal static class SetupServices
{
    public static IServiceCollection SetupUtilityServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IRunner, Runner>()
            .AddSingleton<IOldLogFilesCleaner, OldLogFilesCleaner>()
            .AddSingleton<IOptionsParser, OptionsParser>()
            .AddCommandParsers()
            .AddCommands();

        return services;
    }

    private static IServiceCollection AddCommandParsers(this IServiceCollection services)
    {
        services.AddSingleton<ICommandParserFactory, CommandParserFactory>();
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

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<ICommandFactory, CommandFactory>();

        var commandType = typeof(ICommand);
        var types = commandType
                    .Assembly
                    .GetTypes()
                    .Where(type =>
                               commandType.IsAssignableFrom(type)
                               && type is { IsInterface: false, IsAbstract: false, }
                    );

        foreach (var type in types)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}