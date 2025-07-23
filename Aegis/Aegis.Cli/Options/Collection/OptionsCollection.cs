using System.Collections.Immutable;
using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Options.Collection;

internal sealed class OptionsCollection(ImmutableArray<IOption> options) : IOptionsCollection
{
    public static readonly OptionsCollection Empty = new([]);

    private readonly ImmutableArray<IOption> _options = options;

    // public TOption Get<TOption>()
    //     where TOption : IOption
    // {
    //     
    // }
}