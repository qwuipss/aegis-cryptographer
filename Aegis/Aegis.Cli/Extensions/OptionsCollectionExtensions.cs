using Aegis.Cli.Exceptions.Algorithms;
using Aegis.Cli.Exceptions.Options;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Collection;
using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Algorithms;

namespace Aegis.Cli.Extensions;

internal static class OptionsCollectionExtensions
{
    public static AlgorithmType GetAlgorithmTypeOrDefault(this IOptionsCollection options)
    {
        var option = options.GetOption<AlgorithmOption>();

        if (option is null)
        {
            return AlgorithmType.Rune;
        }

        if (Enum.TryParse<AlgorithmType>(option.Value, true, out var algorithmType))
        {
            return algorithmType;
        }

        throw new AlgorithmNotRecognizedException(option.Value);
    }

    public static void ShouldContainOnlyOptions<T1>(this IOptionsCollection options)
        where T1 : class, IOption
    {
        options.ShouldContainOnlyOptions(typeof(T1));
    }

    public static void ShouldNotContainAnyOptions(this IOptionsCollection options)
    {
        
    }

    private static void ShouldContainOnlyOptions(this IOptionsCollection options, params Type[] optionTypes)
    {
        foreach (var optionType in optionTypes)
        {
            if (!optionType.IsAssignableTo(typeof(IOption)))
            {
                throw new ArgumentException(
                    $"Option type '{optionType.Name}' should be assignable from '{nameof(IOption)}'",
                    nameof(optionTypes)
                );
            }
        }

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