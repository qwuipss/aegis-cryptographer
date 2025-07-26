using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options;

internal class NumberOption(OptionKey key, string value) : BaseOption<int>(key)
{
    public override int Value { get; } = int.TryParse(value, out var result) ? result : throw new OptionValueConvertToNumberException(value);
}