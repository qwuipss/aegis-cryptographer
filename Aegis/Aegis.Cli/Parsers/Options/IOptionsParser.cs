using System.Collections.Immutable;
using Aegis.Cli.Options;

namespace Aegis.Cli.Parsers.Options;

internal interface IOptionsParser
{
    (IOptionsCollection Options, int Index) Parse(ImmutableArray<string> args, int index);
}