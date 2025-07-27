using System.Collections.Immutable;
using Aegis.Cli.Exceptions.Commands;

namespace Aegis.Cli.Extensions;

internal static class CommandExtensions
{
    public static void ShouldContainSingleParameter(this ImmutableArray<string> parameters)
    {
        if (parameters.Length is not 1)
        {
            throw new CommandParametersCountMismatch(1, parameters.Length);
        }
    }
}