namespace Aegis.Cli.Setup;

internal static class GlobalsSetup
{
    public static void SetupUtilityGlobals()
    {
        AppContext.SetData(GlobalsKeys.ExecutionId, Guid.NewGuid().ToString("n"));
    }
}