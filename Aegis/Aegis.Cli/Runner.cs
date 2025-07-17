using Aegis.Cli.Exceptions;
using Aegis.Cli.Parsers;
using Aegis.Cli.Services;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli;

internal sealed class Runner(
    ILogger<Runner> logger,
    IOldLogFilesCleaner cleaner,
    ICommandParser parser
)
    : IRunner
{
    private readonly ILogger<Runner> _logger = logger;
    private readonly IOldLogFilesCleaner _cleaner = cleaner;
    private readonly ICommandParser _parser = parser;

    public Task RunAsync(string[] args)
    {
        try
        {
            var command = _parser.Parse([..args,], 0);

            _logger.LogInformation("Executing command '{commandType}'", command.GetType().Name);

            command.Execute();
        }
        catch (IntentionalException exc)
        {
            _logger.LogError(exc, "{intentionalExceptionMessage}", exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occured while running command");
        }

        return Task.CompletedTask;

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