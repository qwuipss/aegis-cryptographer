using Aegis.Cli.Options.Abstract;

namespace Aegis.Cli.Options.Factory;

internal interface IOptionFactory
{
    bool TryCreate(string key, string? value, out IOption? option);
}