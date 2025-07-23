using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options.Abstract;

internal abstract class StringOption : BaseOption
{
    public string Value { get; private set; } = null!;

    public override void Initialize(string key, string? value)
    {
        if (value is null)
        {
            throw new OptionValueIsNullException(key);
        }
        
        base.Initialize(key, value);
        Value = value;
    }
}