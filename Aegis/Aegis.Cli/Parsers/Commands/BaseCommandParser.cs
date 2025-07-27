using System.Collections.Immutable;
using Aegis.Cli.Commands;
using Aegis.Cli.Options.Collection;
using Aegis.Cli.Parsers.Options;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Parsers.Commands;

internal abstract class BaseCommandParser(ILogger logger, IOptionsParser optionsParser) : ICommandParser
{
    protected readonly ILogger Logger = logger;
    
    private readonly IOptionsParser _optionsParser = optionsParser;

    public abstract ICommand Parse(ImmutableArray<string> args, int index);

    protected (IOptionsCollection Options, int ParametersIndex) ParseOptions(ImmutableArray<string> args, int index)
    {
        Logger.LogDebug("Parsing options");
        return _optionsParser.Parse(args, index);
    }
}