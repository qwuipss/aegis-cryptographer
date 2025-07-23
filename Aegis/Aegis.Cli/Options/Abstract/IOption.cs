namespace Aegis.Cli.Options.Abstract;

internal interface IOption
{
    void Initialize(string key, string? value);

    void Validate();
}