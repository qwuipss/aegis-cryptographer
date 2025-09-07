using System.Text;
using Aegis.Cli.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Interaction;

internal sealed class ConsoleReader(ILogger<ConsoleReader> logger, ILogger<InlineLogger> inlineLogger) : IConsoleReader
{
    private readonly ILogger<ConsoleReader> _logger = logger;
    private readonly ILogger<InlineLogger> _inlineLogger = inlineLogger;

    public string ReadSecret(string caption)
    {
        _inlineLogger.LogInformation("{caption}: ", caption);

        var secretBuilder = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (secretBuilder.Length == 0)
                {
                    continue;
                }

                secretBuilder.Length -= 1;
            }
            else
            {
                var c = key.KeyChar;
                if (char.IsControl(c))
                {
                    continue;
                }

                secretBuilder.Append(c);
            }
        }

        var secretString = secretBuilder.ToString();

#if DEBUG
        _logger.LogDebug("Read: {secret}", secretString);
#else
        _logger.LogDebug("Read");
#endif
        return secretString;
    }
}