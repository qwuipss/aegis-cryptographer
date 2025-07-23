using System.Reflection;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Attributes;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Options.Factory;

internal sealed class OptionFactory(ILogger<OptionFactory> logger) : IOptionFactory
{
    private static readonly Dictionary<string, Type> OptionTypes = new();

    private readonly ILogger<OptionFactory> _logger = logger;

    static OptionFactory()
    {
        var optionType = typeof(IOption);

        var options = optionType
                      .Assembly
                      .GetTypes()
                      .Where(type => type is { IsAbstract: false, IsInterface: false, }
                                     && type.IsAssignableTo(optionType)
                      );

        foreach (var option in options)
        {
            var shortAttribute = option.GetCustomAttribute<OptionShortTokenAttribute>();
            var longAttribute = option.GetCustomAttribute<OptionLongTokenAttribute>();

            OptionTypes[$"{OptionMarkup.ShortTokenPrefix}{shortAttribute!.Token}"] = optionType;
            OptionTypes[$"{OptionMarkup.LongTokenPrefix}{longAttribute!.Token}"] = optionType;
        }
    }

    public bool TryCreate(string key, string? value, out IOption? option)
    {
        if (OptionTypes.TryGetValue(key, out var type))
        {
            _logger.LogDebug("Creating option '{optionName}'", type.Name);
            option = (IOption)Activator.CreateInstance(type)!;

            _logger.LogDebug("Initializing option");
            option.Initialize(key, value);

            _logger.LogDebug("Validating option");
            option.Validate();

            return true;
        }

        option = null;
        return false;
    }
}