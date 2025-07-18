namespace Aegis.Cli.Options;

internal interface IOption
{
    public string Value { get; }

    void Validate();
}