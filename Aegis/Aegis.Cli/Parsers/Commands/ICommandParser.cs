using System.Collections.Immutable;
using Aegis.Cli.Commands;

namespace Aegis.Cli.Parsers.Commands;

internal interface ICommandParser
{
    ICommand Parse(ImmutableArray<string> parameters, int index);
}