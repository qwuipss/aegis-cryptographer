using System.Collections.Immutable;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Collection;
using Aegis.Cli.Parsers.Options;

namespace Aegis.Cli.Commands.Factory;

internal interface ICommandFactory
{
    TCommand Create<TCommand>(ImmutableArray<string> parameters, IOptionsCollection options) where TCommand : ICommand;
}