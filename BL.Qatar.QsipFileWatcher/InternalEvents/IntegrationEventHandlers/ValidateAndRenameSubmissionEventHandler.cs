using BL.Internal.Messaging.Interfaces;
using BL.Qatar.QsipFileWatcher.ExternalEvents;
using BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEvents;
using BL.Qatar.QsipFileWatcher.Model;
using BL.Qatar.QsipFileWatcher.Models;
using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEventHandlers;

public class ValidateAndRenameSubmissionEventHandler(
    IInMemoryEventBus inMemoryEventBus, IEventBus externalEventBus,
        ILogger<ValidateAndRenameSubmissionEventHandler> logger) : INotificationHandler<ValidateAndRenameSubmissionEvent>
{
    public async Task Handle(ValidateAndRenameSubmissionEvent @event, CancellationToken cancellation)
    {
        Console.WriteLine("Handling the ValidateAndRenameSubmissionEvent..." + @event.Id);
        var dirPath = Path.GetDirectoryName(@event.csvPath);
        if (Directory.Exists(dirPath))
        {
            if (IsDirectoryWritable(dirPath))
            {
                var newFilename = String.Concat("$_", Path.GetFileName(@event.csvPath));
                try
                {
                    // Wait for the file to be completely copied
                    await WaitForFileToBeReady(@event.csvPath);

                    File.Move(@event.csvPath, Path.Combine(dirPath, newFilename));
                    // TODO: Validate the CSV
                    // Write method to valid date csv returning isValid true or false
                    // Only move forward if all is successful

                    //// await inMemoryEventBus.PublishAsync(new CreateMetsEvent(@event.Id, @event.csvPath));

                    //Loop through the images in the submission and send them to be transformed

                    //But in the meantime we just go onto the next worker
                    var newQsipDataModel = new QSipPackageDataModel(Path.Combine(dirPath, newFilename));
                    var myQsipPackageRequestEvent = new QsipPackageRequestIntegrationEvent(newQsipDataModel);
                    await Task.Run(() => externalEventBus.Publish(myQsipPackageRequestEvent));
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception thrown when attempting to rename file. Exception message:{ex.Message}");
                }
            }
            else
            {
                logger.LogError($"The Submission Folder is not writable. Event Id:{@event.Id}");
            }
        }
        else
        {
            logger.LogError($"The Submission Folder Does not exist. Event Id:{@event.Id}");
        }
    }

    private async Task WaitForFileToBeReady(string filePath)
    {
        while (true)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (stream.Length > 0)
                    {
                        break;
                    }
                }
            }
            catch (IOException)
            {
                await Task.Delay(500);
            }
        }
    }

    private bool IsDirectoryWritable(string dirPath)
    {
        try
        {
            using (FileStream fs = File.Create(
                Path.Combine(
                    dirPath,
                    Path.GetRandomFileName()
                ),
                1,
                FileOptions.DeleteOnClose)
            )
            { }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
