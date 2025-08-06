using System.Text;

namespace Aegis.Cli;

internal static class Globals
{
    private const string ExecutionIdKey = "ExecutionId";

    public static readonly string ExecutionId = (string)AppContext.GetData(ExecutionIdKey)!;
    public static readonly Encoding ConsoleEncoding = Encoding.UTF8;

    public static void Setup()
    {
        Console.InputEncoding = Console.OutputEncoding = ConsoleEncoding;
        AppContext.SetData(ExecutionIdKey, Guid.NewGuid().ToString("n"));
    }
}