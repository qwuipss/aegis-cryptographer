namespace Aegis.Cli.Setup;

internal class GlobalsSetup
{
    public static void SetupUtilityGlobals()
    {
        AppContext.SetData(GlobalsKeys.ExecutionId, Guid.NewGuid().ToString("n"));
    }
}