using System.Reflection;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Attributes;

namespace Aegis.Cli.Setup;

internal static class ValidatorsSetup
{
    public static void SetupValidators()
    {
        var optionType = typeof(IOption);

        var options = optionType
                      .Assembly
                      .GetTypes()
                      .Where(type => type is { IsAbstract: false, IsInterface: false, }
                                     && type.IsAssignableTo(optionType)
                      );

        var shortTokens = new Dictionary<string, Type>();
        var longTokens = new Dictionary<string, Type>();

        foreach (var option in options)
        {
            var hasCtor = option.GetConstructor(Type.EmptyTypes);
            if (hasCtor is null)
            {
                throw new ArgumentException($"Option '{option.Name}' has no parameterless constructor");
            }

            var shortAttribute = option.GetCustomAttribute<OptionShortTokenAttribute>();
            if (shortAttribute is null)
            {
                throw new ArgumentException($"Option '{option.Name}' is not marked by '{nameof(OptionShortTokenAttribute)}'");
            }

            if (shortTokens.TryGetValue(shortAttribute.Token, out var shortAttributeDuplicate))
            {
                throw new ArgumentException($"Token '{shortAttribute.Token}' registered for both '{option.Name}' and '{shortAttributeDuplicate.Name}'");
            }

            shortTokens.Add(shortAttribute.Token, option);

            var longAttribute = option.GetCustomAttribute<OptionLongTokenAttribute>();
            if (longAttribute is null)
            {
                throw new ArgumentException($"Option '{option.Name}' is not marked by '{nameof(OptionLongTokenAttribute)}'");
            }

            if (longTokens.TryGetValue(longAttribute.Token, out var longAttributeDuplicateType))
            {
                throw new ArgumentException($"Token '{shortAttribute.Token}' registered for both '{option.Name}' and '{longAttributeDuplicateType.Name}'");
            }

            longTokens.Add(longAttribute.Token, option);
        }
    }
}