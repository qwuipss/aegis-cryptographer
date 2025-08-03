using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options.Abstract;

internal abstract class NumberOption(string value) : BaseOption<int>()
{
    public override int Value { get; } = int.TryParse(value, out var result) ? result : throw new OptionValueConvertToNumberException(value);
}