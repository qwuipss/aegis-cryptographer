namespace Aegis.Cli.Options.Abstract;

internal abstract class StringOption( string value) : BaseOption<string>()
{
    public override string Value { get; } = value;
}