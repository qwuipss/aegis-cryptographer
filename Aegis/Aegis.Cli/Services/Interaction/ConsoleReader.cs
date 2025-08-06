using System.Collections.Immutable;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services.Interaction;

internal sealed class ConsoleReader(ILogger<ConsoleReader> logger) : IConsoleReader
{
    private readonly ILogger<ConsoleReader> _logger = logger;

    public ImmutableArray<byte> ReadSecret()
    {
        var secret = new List<byte>();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (secret.Count <= 0)
                {
                    continue;
                }

                secret.RemoveAt(secret.Count - 1);
            }
            else
            {
                var c = key.KeyChar;
                if (char.IsControl(c))
                {
                    continue;
                }

                var bytes = Globals.ConsoleEncoding.GetBytes([c,]);
                secret.AddRange(bytes);
            }
        }

#if DEBUG
        _logger.LogDebug("Secret read: {secret}", Globals.ConsoleEncoding.GetString(secret.ToArray()));
#else
        _logger.LogDebug("Secret read");
#endif
        return [..secret,];
    }
}