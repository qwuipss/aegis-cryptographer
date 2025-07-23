using System.Collections.Immutable;
using Aegis.Cli.Options;
using Aegis.Cli.Options.Abstract;
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

        // return (OptionsCollection.Empty, index);
        var optionsList = new List<IOption>();
        while (index < args.Length)
        {
            var token = args[index];

            if (token == OptionMarkup.OptionsTerminateToken)
            {
                index++;
                break;
            }

            if (token.StartsWith(OptionMarkup.LongTokenPrefix))
            {
            }
            else if (token.StartsWith(OptionMarkup.ShortTokenPrefix))
            {
            }
            else
            {
                break;
            }

            if (args.Length <= index)
            {
                // single
            }

            var value = args[index];
        }
    }
}