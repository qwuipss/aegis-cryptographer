namespace Aegis.Cli.Options.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class OptionShortTokenAttribute(string token) : Attribute
{
    public string Token { get; } = token;
}