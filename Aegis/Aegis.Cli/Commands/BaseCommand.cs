using System.Collections.Immutable;
using Aegis.Cli.Options;

namespace Aegis.Cli.Commands;

internal abstract class BaseCommand : ICommand
{
    protected ImmutableArray<string> Parameters { get; private set; }

    protected IOptionsCollection Options { get; private set; } = null!;

    public void Initialize(ImmutableArray<string> parameters, IOptionsCollection options)
    {
        Parameters = parameters;
        Options = options;
    }

    public abstract void Validate();

    public abstract Task ExecuteAsync();
}