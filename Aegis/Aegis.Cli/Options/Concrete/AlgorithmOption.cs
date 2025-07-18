namespace Aegis.Cli.Options.Concrete;

internal sealed class AlgorithmOption(string value) : IOption
{
    public string Value { get; } = value;

    public void Validate()
    {
    }
}