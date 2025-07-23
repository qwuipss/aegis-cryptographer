using System.Collections.Immutable;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands.Factory;

internal sealed class CommandFactory(ILogger<CommandFactory> logger, IServiceProvider serviceProvider) : ICommandFactory
{
    private readonly ILogger<CommandFactory> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TCommand Create<TCommand>(ImmutableArray<string> parameters, IOptionsCollection options)
        where TCommand : ICommand
    {
        _logger.LogDebug("Creating command '{commandType}'", typeof(TCommand).Name);
        var command = _serviceProvider.GetRequiredService<TCommand>();

        _logger.LogDebug("Initializing command");
        command.Initialize(parameters, options);

        _logger.LogDebug("Validating command");
        command.Validate();

        return command;
    }
}