namespace Aegis.Cli.Options;

internal abstract class BaseOption<TValue>(OptionKey key) : IOption
{
    public abstract TValue Value { get; }

    public OptionKey Key { get; } = key;

    public override bool Equals(object? obj)
    {
        return obj is IOption option && option.Key == Key;
    }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}