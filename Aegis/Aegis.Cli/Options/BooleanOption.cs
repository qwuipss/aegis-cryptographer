using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options;

internal class BooleanOption(OptionKey key, string? value) : BaseOption<bool>(key)
{
    public override bool Value { get; } = value is null
                                 || (bool.TryParse(value, out var result)
                                     ? result
                                     : throw new OptionValueConvertToBooleanException(value));
}