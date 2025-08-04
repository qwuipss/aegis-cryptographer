using System.Text;

namespace Aegis.Cli.Services.Interaction;

internal sealed class ConsoleReader : IConsoleReader
{
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
                if (secret.Length > 0)
                {
                    secret.Length--;
                }
            }
            else
            {
                secret.Append(key.KeyChar);
            }
        }

        return secret.ToString();
    }
}