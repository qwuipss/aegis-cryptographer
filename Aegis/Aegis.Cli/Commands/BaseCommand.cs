using System.Collections.Immutable;
using Aegis.Cli.Options.Collection;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Commands;

internal abstract class BaseCommand(ILogger logger) : ICommand
{
    private bool _isInitialized;

    protected ImmutableArray<string> Parameters { get; private set; }

    protected IOptionsCollection Options { get; private set; } = null!;

    protected ILogger Logger { get; } = logger;

    public virtual void Initialize(ImmutableArray<string> parameters, IOptionsCollection options)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Command already initialized");
        }

        _isInitialized = true;

        Parameters = parameters;
        Options = options;

        Logger.LogDebug("Received parameters: {parametersCount}. Received options: {optionsCount}", parameters.Length, Options.Count);
    }

    public abstract void Validate();

    public abstract Task ExecuteAsync();
}