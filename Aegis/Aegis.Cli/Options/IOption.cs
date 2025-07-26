namespace Aegis.Cli.Options;

internal interface IOption
{
    OptionKey Key { get; }
    void Validate();
}