using Aegis.Cli.Exceptions.Options;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Collection;

namespace Aegis.Cli.Extensions;

internal static class OptionExtensions
{
    public static void ShouldContainOnlyOptions<T1>(this IOptionsCollection options)
        where T1 : class, IOption
    {
        options.ShouldContainOnlyOptions(typeof(T1));
    }

    private static void ShouldContainOnlyOptions(this IOptionsCollection options, params Type[] optionTypes)
    {
        var keysSet = new HashSet<Type>(optionTypes);
        foreach (var option in options)
        {
            if (!keysSet.Contains(option.GetType()))
            {
                throw new OptionNotSupportedByCommandException(option);
            }
        }
    }
}