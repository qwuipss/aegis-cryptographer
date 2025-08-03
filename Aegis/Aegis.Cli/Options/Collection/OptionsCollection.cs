using System.Collections;
using System.Collections.Immutable;
using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Options.Collection;

internal sealed class OptionsCollection(ImmutableHashSet<IOption> options) : IOptionsCollection
{
    public static readonly OptionsCollection Empty = new([]);

    private readonly ImmutableHashSet<IOption> _options = options;

    public int Count => _options.Count;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<IOption> GetEnumerator()
    {
        return _options.GetEnumerator();
    }

    public TOption? GetOption<TOption>() where TOption : class, IOption
    {
        return _options.SingleOrDefault(o => o.GetType() == typeof(TOption)) as TOption;
    }
}