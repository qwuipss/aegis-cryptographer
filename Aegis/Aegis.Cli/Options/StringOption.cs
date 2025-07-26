using Aegis.Cli.Exceptions.Options;

namespace Aegis.Cli.Options;

internal class StringOption(OptionKey key, string value) : BaseOption<string>(key)
{
    public override string Value { get; } = value;
}