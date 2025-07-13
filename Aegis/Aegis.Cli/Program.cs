using Aegis.Cli.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Aegis.Cli;

internal static class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        GlobalsSetup.SetupUtilityGlobals();
        services.SetupUtilityLogging();
        services.SetupUtilityServices();

        using var serviceProvider = services.BuildServiceProvider();
        var runner = serviceProvider.GetRequiredService<IRunner>();

        runner.RunAsync(args);
    }
}