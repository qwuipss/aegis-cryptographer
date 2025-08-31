namespace Aegis.Cli.Extensions;

internal static class StringExtensions
{
    public static MemoryStream ToGlobalEncodingMemoryStream(this string str)
    {
        var bytes = Globals.ConsoleEncoding.GetBytes(str);
        return new MemoryStream(bytes);
    }
    
    public static byte[] ToGlobalEncodingBytes(this string str)
    {
        var bytes = Globals.ConsoleEncoding.GetBytes(str);
        return bytes;
    }
}