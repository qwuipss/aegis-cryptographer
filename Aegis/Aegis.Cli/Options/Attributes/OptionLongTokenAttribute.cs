namespace Aegis.Cli.Options.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class OptionLongTokenAttribute(string token) : Attribute
{
    public string Token { get; } = token;
}