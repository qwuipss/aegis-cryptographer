namespace Aegis.Cli.Options.Abstract;

internal abstract class BaseOption<TValue> : IOption
{
    public abstract TValue Value { get; }
    
    public override bool Equals(object? obj)
    {
        return obj is IOption option && option.GetType() == GetType();
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}