using BL.Internal.Messaging.Interfaces;
using BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEventHandlers;

public class ValidateAndRenameSubmissionEventHandler(
    IInMemoryEventBus inMemoryEventBus, 
        ILogger<ValidateAndRenameSubmissionEventHandler> logger) : INotificationHandler<ValidateAndRenameSubmissionEvent>
{
    public async Task Handle(ValidateAndRenameSubmissionEvent @event, CancellationToken cancellation)
    {
        Console.WriteLine("Handling the ValidateAndRenameSubmissionEvent..." + @event.Id);
        var dirPath = Path.GetDirectoryName(@event.csvPath);
        if (Directory.Exists(dirPath)) {
            if (IsDirectoryWritable(dirPath))
            {
                var newFilename = String.Concat("$_", Path.GetFileName(@event.csvPath));
                try
                {
                    File.Move(@event.csvPath, Path.Combine(dirPath, newFilename));
                    // TODO: Validate the CSV
                    // Write methos to validate csv returning isValid true or false
                    // Only move forward if all is successful
                    await inMemoryEventBus.PublishAsync(new CreateMetsEvent(@event.Id, @event.csvPath));
                }
                catch(Exception ex)
                {
                    logger.LogError($"Exception thrown when attempting to rename file. Exception message:{ex.Message}");
                }
            } else {
                logger.LogError($"The Submission Folder is not writable. Event Id:{@event.Id}");
            }
        } else {
            logger.LogError($"The Submission Folder Does not exist. Event Id:{@event.Id}");
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
