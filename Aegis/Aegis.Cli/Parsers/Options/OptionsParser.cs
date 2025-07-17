using System.Collections.Immutable;
using Aegis.Cli.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Options;

internal sealed class OptionsParser(ILogger<OptionsParser> logger) : IOptionsParser
{
    private readonly ILogger<OptionsParser> _logger = logger;

    public IOptionsCollection Parse(ImmutableArray<string> parameters, int index)
    {
        if (parameters.Length < index + 1)
        {
            _logger.LogDebug("Parameters count: {parametersCount}. Index specified: {index}. No options parsed", parameters.Length, index);
            return OptionsCollection.Empty;
        }

        return OptionsCollection.Empty;
        for (int i = index + 1; i < parameters.Length; i++)
        {
        }
    }
}