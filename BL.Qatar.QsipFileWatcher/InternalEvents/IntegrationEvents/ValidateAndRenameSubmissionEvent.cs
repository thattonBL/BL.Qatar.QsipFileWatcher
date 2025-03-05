using BL.Internal.Messaging.Interfaces;

namespace BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEvents;

public record ValidateAndRenameSubmissionEvent(Guid Id, string csvPath) : InternalEvent(Id); 
