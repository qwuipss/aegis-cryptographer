namespace Aegis.Cli.Options.Abstract;

internal abstract class BaseOption : IOption
{
    private bool _isInitialized;

    public virtual void Initialize(string key, string? value)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Option already initialized");
        }

        _isInitialized = true;
    }

    public virtual void Validate()
    {
    }
}