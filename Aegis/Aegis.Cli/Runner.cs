using Aegis.Cli.Exceptions;
using Aegis.Cli.Parsers;
using Aegis.Cli.Parsers.Commands;
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

    public async Task RunAsync(string[] parameters)
    {
        try
        {
            var command = _parser.Parse([..parameters,], 0);

            _logger.LogInformation("Executing command '{commandType}'", command.GetType().Name);

            await command.ExecuteAsync();
        }
        catch (IntentionalException exc)
        {
            _logger.LogError(exc, "{intentionalExceptionMessage}", exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occured while running command");
        }

        _logger.LogInformation("Execution id: {executionId}", AppContext.GetData(GlobalsKeys.ExecutionId));

        return;

        try
        {
            _cleaner.Clean();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occured while cleaning old log files");
        }

        return;
    }
}