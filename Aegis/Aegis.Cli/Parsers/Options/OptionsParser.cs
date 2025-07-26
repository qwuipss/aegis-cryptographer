using System.Collections.Immutable;
using Aegis.Cli.Exceptions.Options;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Collection;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Options;

internal sealed class OptionsParser(ILogger<OptionsParser> logger) : IOptionsParser
{
    private readonly ILogger<OptionsParser> _logger = logger;

    public (IOptionsCollection Options, int Index) Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length <= index)
        {
            return (OptionsCollection.Empty, index);
        }

        var options = new HashSet<IOption>();
        while (index < args.Length)
        {
            var token = args[index];

            if (token == OptionTokens.OptionsTerminateToken)
            {
                index++;
                break;
            }

            string name;
            if (token.StartsWith(OptionTokens.LongTokenPrefix))
            {
                name = GetOptionName(token, OptionTokens.LongTokenPrefix);
            }
            else if (token.StartsWith(OptionTokens.ShortTokenPrefix))
            {
                name = GetOptionName(token, OptionTokens.ShortTokenPrefix);
            }
            else
            {
                break;
            }

            index++;
            if (index == args.Length)
            {
                break;
            }

            var value = args[index];

            if (value.StartsWith(OptionTokens.LongTokenPrefix) || value.StartsWith(OptionTokens.ShortTokenPrefix))
            {
                CreateAndAddOptionWithoutValue(name, options);
                continue;
            }

            var option = CreateOption(name, value);
            options.Add(option);
            continue;

            static void CreateAndAddOptionWithoutValue(string name, HashSet<IOption> options)
            {
                var option = CreateOption(name, null);
                options.Add(option);
            }
        }

        return (new OptionsCollection(options.ToImmutableHashSet()), index);
    }

    private static string GetOptionName(string token, string prefix)
    {
        if (token.Length == prefix.Length)
        {
            throw new OptionNameNotParsedException(token);
        }

        return token[prefix.Length..];
    }

    private static IOption CreateOption(string name, string? value)
    {
        IOption option = name switch
        {
            OptionTokens.Algorithm.ShortToken or OptionTokens.Algorithm.LongToken
                => new StringOption(OptionKey.Algorithm, value ?? throw new OptionValueIsNullException(name)),
        };

        option.Validate();

        return option;
    }
}