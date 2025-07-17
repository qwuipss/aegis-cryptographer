using System.Collections.Immutable;

namespace Aegis.Cli.Extensions;

internal static class CommandExtensions
{
    public static void ShouldContainSingleParameter(this ImmutableArray<string> parameters)
    {
        if (parameters.Length is not 1)
        {
            throw new ArgumentOutOfRangeException(nameof(parameters), "Parameters collection should contain single parameter only");
        }
    }
}