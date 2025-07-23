using Aegis.Cli.Options.Abstract;
using Aegis.Cli.Options.Attributes;

namespace Aegis.Cli.Options.Concrete;

[OptionShortToken("alg")]
[OptionLongToken("algorithm")]
internal sealed class AlgorithmOption : StringOption
{
}