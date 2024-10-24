using AegisCryptographer.Configuration;

namespace AegisCryptographer.IO;

public class Writer : IWriter
{
    public Writer(IConfigurationProvider configurationProvider)
    {
        Console.InputEncoding = configurationProvider.Encoding;
    }

    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public void Write(string text)
    {
        Console.Write(text);
    }

    public void WriteException(Exception exception)
    {
        Console.WriteLine($"Error: {exception.Message}");
    }

    public void WriteEnterSecret()
    {
        Console.Write("Secret: ");
    }

    public void WriteRepeatSecret()
    {
        Console.Write("Repeat secret: ");
    }
}