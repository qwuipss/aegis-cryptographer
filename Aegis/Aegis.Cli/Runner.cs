using System.Collections.Immutable;
using Aegis.Cli.Exceptions;
using Aegis.Cli.Parsers;
using Aegis.Cli.Services;
using Aegis.Cli.Services.Logging;
using Aegis.Cli.Utilities;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli;

internal sealed class Runner(
    ILogger<Runner> logger,
    IOldLogFilesCleaner cleaner,
    ICommandParser parser,
    ISpecialLoggerFactory specialLoggerFactory
)
    : IRunner
{
    private readonly ILogger<Runner> _logger = logger;
    private readonly IOldLogFilesCleaner _cleaner = cleaner;
    private readonly ICommandParser _parser = parser;
    private readonly ISpecialLoggerFactory _specialLoggerFactory = specialLoggerFactory;

    public Task RunAsync(string[] args)
    {
        try
        {
            var command = _parser.Parse([..args,], 0);

            var result = command.Execute();

            var secretLogger = _specialLoggerFactory.CreateSecretLogger();
            
            secretLogger.LogInformation("{commandResult}", result.DisplayText);
        }
        catch (IntentionalException exc)
        {
            _logger.LogError(exc, "{message}", exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occured while running command");
        }

        try
        {
            _cleaner.Clean();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occured while cleaning old log files");
        }

        _logger.LogInformation("Execution id: {executionId}", AppContext.GetData(GlobalsKeys.ExecutionId));

        return Task.CompletedTask;
    }
}