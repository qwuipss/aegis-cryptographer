using System.Collections.Immutable;
using Aegis.Cli.Exceptions.Options;
using Aegis.Cli.Exceptions.Parsers.Options;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Collection;
using Aegis.Cli.Options.Concrete;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Options;

internal sealed class OptionsParser(ILogger<OptionsParser> logger) : IOptionsParser
{
    private readonly ILogger<OptionsParser> _logger = logger;

    public (IOptionsCollection Options, int Index) Parse(ImmutableArray<string> args, int index)
    {
        if (args.Length <= index)
        {
            _logger.LogDebug("No options parsed");
            return (OptionsCollection.Empty, index);
        }

        var options = new HashSet<IOption>();
        while (index < args.Length)
        {
            var token = args[index];

            if (token is OptionTokens.OptionsTerminateToken)
            {
                index++;
                break;
            }

            if (!TryParseOptionName(token, out var name))
            {
                break;
            }

            index++;

            IOption option;
            var value = index == args.Length ? null : args[index];
            if (value is not null && IsStartingAsOptionName(value))
            {
                option = CreateOption(token, name!, null);
            }
            else
            {
                option = CreateOption(token, name!, value);
                index++;
            }

            _logger.LogDebug("Parsed option '{optionName}'", option.GetType().Name);

            if (!options.Add(option))
            {
                throw new OptionDuplicateException(option);
            }
        }

        return (new OptionsCollection(options.ToImmutableHashSet()), index);
    }

    private static bool TryParseOptionName(string token, out string? name)
    {
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
            name = null;
            return false;
        }

        if (name.Length is 0)
        {
            throw new OptionNameNotParsedException(token);
        }

        return true;

        static string GetOptionName(string token, string prefix)
        {
            return token[prefix.Length..];
        }
    }

    private static bool IsStartingAsOptionName(string token)
    {
        return token.StartsWith(OptionTokens.LongTokenPrefix) || token.StartsWith(OptionTokens.ShortTokenPrefix);
    }

    private static IOption CreateOption(string token, string name, string? value)
    {
        IOption option = name switch
        {
            OptionTokens.Algorithm.ShortToken or OptionTokens.Algorithm.LongToken
                => new AlgorithmOption(value ?? throw new OptionValueIsNullException(token)),
            _ => throw new OptionNameNotParsedException(token),
        };

        return option;
    }
}