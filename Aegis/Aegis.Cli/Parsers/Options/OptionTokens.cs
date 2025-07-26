namespace Aegis.Cli.Parsers.Options;

internal static class OptionTokens
{
    public const string OptionsTerminateToken = "--";

    public const string ShortTokenPrefix = "-";
    public const string LongTokenPrefix = "--";

    public static class Algorithm
    {
        public const string ShortToken = "alg";
        public const string LongToken = "algorithm";
    }
}