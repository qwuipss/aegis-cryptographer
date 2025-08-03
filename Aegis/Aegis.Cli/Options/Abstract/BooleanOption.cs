using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options.Abstract;

internal abstract class BooleanOption(string? value) : BaseOption<bool>
{
    public override bool Value { get; } = value is null
                                 || (bool.TryParse(value, out var result)
                                     ? result
                                     : throw new OptionValueConvertToBooleanException(value));
}