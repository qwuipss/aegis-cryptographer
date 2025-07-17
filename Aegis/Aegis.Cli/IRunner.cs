namespace Aegis.Cli;

internal interface IRunner
{
    Task RunAsync(string[] parameters);
}