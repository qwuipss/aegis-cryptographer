using System.Collections.Immutable;
using Aegis.Cli.Commands;

namespace Aegis.Cli.Parsers.Commands;

internal interface ICommandParser
{
    ICommand Parse(ImmutableArray<string> args, int index);
}