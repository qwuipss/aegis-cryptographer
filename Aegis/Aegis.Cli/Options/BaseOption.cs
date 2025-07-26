namespace Aegis.Cli.Options;

internal abstract class BaseOption<TValue>(OptionKey key) : IOption
{
    public abstract TValue Value { get; }

    public OptionKey Key { get; } = key;

    public virtual void Validate()
    {
    }
}