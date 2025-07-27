using System.Collections.Immutable;

namespace Aegis.Cli.Options.Collection;

internal sealed class OptionsCollection(ImmutableHashSet<IOption> options) : IOptionsCollection
{
    public static readonly OptionsCollection Empty = new([]);

    public int Count => _options.Count;

    private readonly ImmutableHashSet<IOption> _options = options;
}