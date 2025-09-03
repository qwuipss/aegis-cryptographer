using Aegis.Cli.Commands;
using Aegis.Cli.Commands.Factory;
using Aegis.Cli.Parsers.Commands;
using Aegis.Cli.Parsers.Commands.Factory;
using Aegis.Cli.Parsers.Options;
using Aegis.Cli.Services.Algorithms;
using Aegis.Cli.Services.Files;
using Aegis.Cli.Services.Interaction;
using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli.Setup;

internal static class ServicesSetup
{
    public static IServiceCollection SetupUtilityServices(this IServiceCollection services)
    {
        return services
               .AddRunner()
               .AddAlgorithmServices()
               .AddInteractionServices()
               .AddFileServices()
               .AddOptionsParser()
               .AddCommandParsers()
               .AddCommands();
    }

    private static IServiceCollection AddRunner(this IServiceCollection services)
    {
        return services.AddSingleton<IRunner, Runner>();
    }

    private static IServiceCollection AddOptionsParser(this IServiceCollection services)
    {
        return services.AddSingleton<IOptionsParser, OptionsParser>();
    }

    private static IServiceCollection AddInteractionServices(this IServiceCollection services)
    {
        return services.AddSingleton<IConsoleReader, ConsoleReader>();
    }

    private static IServiceCollection AddAlgorithmServices(this IServiceCollection services)
    {
        return services.AddSingleton<IAlgorithmFactory, AlgorithmFactory>();
    }

    private static IServiceCollection AddFileServices(this IServiceCollection services)
    {
        return services.AddSingleton<IOldLogFilesCleaner, OldLogFilesCleaner>();
    }

    private static IServiceCollection AddCommandParsers(this IServiceCollection services)
    {
        services
            .AddSingleton<ICommandParserFactory, CommandParserFactory>()
            .AddSingleton<ICommandParser, RootCommandParser>();

        var parserType = typeof(ICommandParser);
        var types = parserType
                    .Assembly
                    .GetTypes()
                    .Where(type =>
                               type != typeof(RootCommandParser)
                               && type is { IsInterface: false, IsAbstract: false, }
                               && parserType.IsAssignableFrom(type)
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