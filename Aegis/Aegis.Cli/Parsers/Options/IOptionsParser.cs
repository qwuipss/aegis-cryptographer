using System.Collections.Immutable;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Collection;

namespace Aegis.Cli.Parsers.Options;

internal interface IOptionsParser
{
    (IOptionsCollection Options, int Index) Parse(ImmutableArray<string> args, int index);
}