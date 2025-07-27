using Aegis.Cli.Options;
using Aegis.Cli.Options.Collection;

namespace Aegis.Cli.Extensions;

internal static class OptionExtensions
{
    public static void ShouldContainOnlyOptions(this IOptionsCollection options, params OptionKey[] keys)
    {
        var keysHashSet = new HashSet<OptionKey>(keys);
        
    }
}