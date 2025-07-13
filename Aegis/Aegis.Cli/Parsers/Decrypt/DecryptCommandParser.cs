using System.Collections.Immutable;
using Aegis.Cli.Commands;

namespace Aegis.Cli.Parsers.Decrypt;

internal sealed class DecryptCommandParser : ICommandParser
{
    public ICommand Parse(ImmutableArray<string> args, int index)
    {
        return null;
    }
}