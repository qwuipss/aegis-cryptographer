using System.Collections.Immutable;
using Aegis.Cli.Options;
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

        // return (OptionsCollection.Empty, index);
        var optionsList = new List<IOption>();
        for (int i = index; i < args.Length; i++)
        {
            var token = args[i];

            if (token == OptionTokens.OptionsParsingTerminateToken)
            {
                break;
            }

            if (token.StartsWith(OptionTokens.LongTokenPrefix))
            {
                var key = token[2..];
            }
            else if (token.StartsWith(OptionTokens.ShortTokenPrefix))
            {
                var key = token[1..];
            }
            else
            {
                
            }
        }
    }
}