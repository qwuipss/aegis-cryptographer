namespace Aegis.Cli.Options;

internal sealed class OptionsCollection : IOptionsCollection
{
    public static readonly OptionsCollection Empty = new();

    private OptionsCollection()
    {
    }
    
}