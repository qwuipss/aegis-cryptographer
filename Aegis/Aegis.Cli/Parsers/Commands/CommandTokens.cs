namespace Aegis.Cli.Parsers.Commands;

internal static class CommandTokens
{
    public static class Common
    {
        public static class String
        {
            public const string LongToken = "string";
            public const string ShortToken = "str";
        }
    }

    public static class Encrypt
    {
        public const string LongToken = "encrypt";
        public const string ShortToken = "enc";
    }

    public static class Decrypt
    {
        public const string LongToken = "decrypt";
        public const string ShortToken = "dec";
    }
}