using Aegis.Cli.Utilities;
using Microsoft.Extensions.Logging;

namespace Aegis.Cli.Services;

internal sealed class OldLogFilesCleaner(ILogger<OldLogFilesCleaner> logger) : IOldLogFilesCleaner
{
    private readonly ILogger<OldLogFilesCleaner> _logger = logger;

    public void Clean()
    {
        const int keepLogFilesCount = 7;

        var directoryInfo = new DirectoryInfo(LogsHelper.GetLogsDirectoryPath());
        var fileInfos = directoryInfo
                        .GetFiles($"*.{LogsHelper.LogFileExtension}", SearchOption.TopDirectoryOnly)
                        .ToList();

        var isCurrentExecutionLogFileCreated = fileInfos.Select(f => f.Name).Contains(LogsHelper.GetLogFileName());

        _logger.LogDebug(
            "Log files found: {filesFoundCount}. Keep log files count: {filesKeepCount}. "
            + "Log file for current execution created: {isCurrentExecutionLogFileCreated}",
            fileInfos.Count,
            keepLogFilesCount,
            isCurrentExecutionLogFileCreated.ToString()
        );

        var totalLogFilesCount = isCurrentExecutionLogFileCreated ? fileInfos.Count : fileInfos.Count + 1;
        if (totalLogFilesCount <= keepLogFilesCount)
        {
            _logger.LogDebug("No cleaning required");
            return;
        }

        var filesToDeleteCount = isCurrentExecutionLogFileCreated
            ? fileInfos.Count - keepLogFilesCount
            : fileInfos.Count - keepLogFilesCount + 1;

        _logger.LogDebug("Cleaning is started. Pending deletion of {filesCount} file(s)", filesToDeleteCount);

        foreach (var fileInfo in fileInfos
                                 .OrderBy(f => f.CreationTime)
                                 .Take(filesToDeleteCount))
        {
            _logger.LogDebug(
                "Deleting file '{fileName}'. File creation time: {creationTime}",
                fileInfo.Name,
                fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
            );

            try
            {
                fileInfo.Delete();
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, "Unable to delete file '{fileName}'", fileInfo.Name);
                continue;
            }

            _logger.LogDebug("File '{fileName}' deleted", fileInfo.Name);
        }

        _logger.LogDebug("Cleaning is finished");
    }
}