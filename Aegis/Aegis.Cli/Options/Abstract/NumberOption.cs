using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options.Abstract;

internal abstract class NumberOption : BaseOption
{
    public int Value { get; private set; }

    public override void Initialize(string key, string? value)
    {
        base.Initialize(key, value);
        Value = int.TryParse(value, out var result) ? result : throw new OptionValueConvertToNumberException(value);
    }
}