using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Options.Collection;

internal interface IOptionsCollection : IEnumerable<IOption>
{
    int Count { get; }

    TOption? GetOption<TOption>()
        where TOption : class, IOption;
}