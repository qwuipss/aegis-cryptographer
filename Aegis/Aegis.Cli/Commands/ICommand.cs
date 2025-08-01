using System.Collections.Immutable;
using Aegis.Cli.Options.Collection;

namespace Aegis.Cli.Commands;

internal interface ICommand
{
    void Initialize(ImmutableArray<string> parameters, IOptionsCollection options);
    void Validate();
    Task ExecuteAsync();
}