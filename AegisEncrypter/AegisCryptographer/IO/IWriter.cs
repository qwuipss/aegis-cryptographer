namespace AegisCryptographer.IO;

public interface IWriter
{
    public void WriteLine();
    public void Write(string text);
    public void WriteException(Exception exception);
    public void WriteEnterSecret();
    public void WriteRepeatSecret();
}