using System.Text;

namespace Aegis.Cli;

internal static class Globals
{
    public static readonly string ExecutionId = Guid.NewGuid().ToString("n");
    public static readonly Encoding ConsoleEncoding = Encoding.UTF8;

    public static void Setup()
    {
        Console.InputEncoding = Console.OutputEncoding = ConsoleEncoding;
    }
}