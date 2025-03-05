using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using BL.Internal.Messaging.Interfaces;
using BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEvents;

public class FileWatcherService(IInMemoryEventBus eventBus)
{
    private FileSystemWatcher? _watcher;

    public void StartWatching(IConfiguration config)
    {
        var path = config["FileWatchDirectory"];

        if (!string.IsNullOrEmpty(path))
        {
            _watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.csv",
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            _watcher.Created += OnChanged;
            Console.WriteLine($"Watching directory: {path}");
        }
        else
        {
            Console.WriteLine("FileWatchDirectory not configured.");
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (!Path.GetFileName(e.Name).StartsWith("$_"))
        {
            Console.WriteLine($"File detected: {e.FullPath}");

            eventBus.PublishAsync(new ValidateAndRenameSubmissionEvent(Guid.NewGuid(), e.FullPath));
        }
    }
}
