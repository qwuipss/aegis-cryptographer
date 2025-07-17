using System.Collections.Immutable;
using Aegis.Cli.Options;

namespace Aegis.Cli.Parsers.Options;

internal interface IOptionsParser
{
    IOptionsCollection Parse(ImmutableArray<string> parameters, int index);
}