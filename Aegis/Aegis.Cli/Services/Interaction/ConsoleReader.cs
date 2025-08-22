using System.Text;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Interaction;

internal sealed class ConsoleReader(ILogger<ConsoleReader> logger) : IConsoleReader
{
    private readonly ILogger<ConsoleReader> _logger = logger;

    public string ReadSecret()
    {
        var secret = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (secret.Length == 0)
                {
                    continue;
                }

                secret.Length -= 1;
            }
            else
            {
                var c = key.KeyChar;
                if (char.IsControl(c))
                {
                    continue;
                }
                
                secret.Append(c);
            }
        }

        var secretString = secret.ToString();
        
#if DEBUG
        _logger.LogDebug("Secret read: {secret}", secretString);
#else
        _logger.LogDebug("Secret read");
#endif
        return secretString;
    }
}