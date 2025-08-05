using System.Collections.Immutable;
using Aegis.Cli.Exceptions.Commands;

namespace Aegis.Cli.Extensions;

internal static class CommandExtensions
{
    public static void ShouldNotContainParameters(this ImmutableArray<string> parameters)
    {
        if (parameters.Length is not 0)
        {
            throw new CommandParametersCountMismatch(0, parameters.Length);
        }
    }
}