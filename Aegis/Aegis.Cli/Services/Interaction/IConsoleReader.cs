using System.Collections.Immutable;

namespace Aegis.Cli.Services.Interaction;

internal interface IConsoleReader
{
    ImmutableArray<byte> ReadSecret();
}