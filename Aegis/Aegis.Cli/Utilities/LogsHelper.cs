namespace Aegis.Cli.Utilities;

internal static class LogsHelper
{
    public const string LogFileExtension = "log";

    public static string GetLogFilePath() =>
        Path.Join(GetLogsDirectoryPath(), GetLogFileName());

    public static string GetLogFileName() =>
        $"{AppContext.GetData(GlobalsKeys.ExecutionId)}.{LogFileExtension}";

    public static string GetLogsDirectoryPath() =>
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aegis", "logs");
}