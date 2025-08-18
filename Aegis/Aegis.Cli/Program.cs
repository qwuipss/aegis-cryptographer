using Aegis.Cli.Options.Concrete;
using Aegis.Cli.Services.Interaction;
using Aegis.Cli.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        Globals.Setup();

        services
            .SetupUtilityLogging()
            .SetupUtilityServices();

        await using var serviceProvider = services.BuildServiceProvider();
        var runner = serviceProvider.GetRequiredService<IRunner>();

        await runner.RunAsync(args);
    }
}