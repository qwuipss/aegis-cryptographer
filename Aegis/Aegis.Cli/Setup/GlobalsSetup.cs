using System.Text;

namespace Aegis.Cli.Setup;

internal static class GlobalsSetup
{
    public static void SetupUtilityGlobals()
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        AppContext.SetData(GlobalsKeys.ExecutionId, Guid.NewGuid().ToString("n"));
    }
}